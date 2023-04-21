using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LBApp.Models;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;

namespace LBApp.Controllers
{
    //[Authorize(Roles="admin")]
    public class GenresController : Controller
    {
        private readonly DblibraryContext _context;

        public GenresController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Genres
        public async Task<IActionResult> Index()
        {
            return _context.Genres != null ?
                        View(await _context.Genres.ToListAsync()) :
                        Problem("Entity set 'DblibraryContext.Genres'  is null.");
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.GenreId == id);
            if (genre == null)
            {
                return NotFound();
            }

            //return View(genre);
            return RedirectToAction("Index", "Books", new { id = genre.GenreId, name = genre.GenreName });
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GenreId,GenreName,GenreDescr")] Genre genre)
        {
            if (_context.Genres.Where(b => genre.GenreName == b.GenreName).Count() > 0)
            {
                ModelState.AddModelError("GenreName", "Жанр з таким ім'ям вже існує");
            }
            if (ModelState.IsValid)
            {
                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GenreId,GenreName,GenreDescr")] Genre genre)
        {
            if (id != genre.GenreId)
            {
                return NotFound();
            }
            var namee = _context.Genres.Where(b => genre.GenreName == b.GenreName);
            if (namee.Count() > 0 && namee.Where(b => b.GenreId == id).Count() == 0)
            {
                ModelState.AddModelError("GenreName", "Жанр з таким ім'ям вже існує");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genre.GenreId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.GenreId == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Genres == null)
            {
                return Problem("Entity set 'DblibraryContext.Genres'  is null.");
            }
            var genre = await _context.Genres.FindAsync(id);
            if (_context.Books.Where(b => b.GenreId == genre.GenreId).Count() > 0)
            {
                //return Problem("У цього автора є книги");
                ModelState.AddModelError("GenreId", "Неможливо видалити, існують книги цього жанру");
                return View(genre);
            }
            if (genre != null)
            {
                _context.Genres.Remove(genre);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenreExists(int id)
        {
            return (_context.Genres?.Any(e => e.GenreId == id)).GetValueOrDefault();
        }


        public bool check(string s1, string s2)
        {
            string[] ss1 = s1.Split(' ');
            string[] ss2 = s2.Split(' ');
            Array.Sort(ss1);
            Array.Sort(ss2);
            return ss1.SequenceEqual(ss2);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {

            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream))
                        {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (var wst in workBook.Worksheets)
                            {
                                Genre genre = new Genre();
                                var f = (from t in _context.Genres where t.GenreName.Contains(wst.Name) select t).FirstOrDefault();
                                if (f != default(Genre))
                                {
                                    genre = f;
                                }
                                else
                                {
                                    genre.GenreName = wst.Name;
                                    genre.GenreDescr = "Genre from Excel";
                                    _context.Genres.Add(genre);

                                }

                                foreach (IXLRow row in wst.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Book book = new Book();
                                        book.BookName = row.Cell(1).Value.ToString();
                                        book.BookPagesCount = Convert.ToInt32(row.Cell(2).Value.ToString());
                                        book.BookPrice = Convert.ToInt32(row.Cell(3).Value.ToString());
                                        book.BookYear = Convert.ToInt32(row.Cell(4).Value.ToString());
                                        book.Genre = genre;
                                        int flag = 0;
                                        if (_context.Books.Where(b => book.BookName == b.BookName).Count() > 0)
                                        {
                                            flag = 1; 
                                        }
                                        if (book.BookYear < 0 || book.BookYear > DateTime.Now.Year)
                                        {
                                            flag = 1; 
                                        }
                                        if (book.BookPrice < 0)
                                        {
                                            flag = 1;
                                        }
                                        if (book.BookPagesCount < 0)
                                        {
                                            flag = 1;
                                        }
                                        if (flag==0) { _context.Books.Add(book);

                                            int start = 5;
                                            int fin = 7;
                                            for (int i = start; i <= fin; i++)
                                            {
                                                if (row.Cell(i).Value.ToString().Length > 0)
                                                {
                                                    Models.Author auth = new Models.Author();
                                                    var a = (from aut in _context.Authors where aut.AuthorName.Contains(row.Cell(i).Value.ToString()) select aut).FirstOrDefault();
                                                    if (a != default(Models.Author))
                                                    {
                                                        auth = a;
                                                    }
                                                    else
                                                    {
                                                        auth.AuthorName = row.Cell(i).Value.ToString();
                                                        int flag2 = 0;
                                                        foreach (var ii in _context.Authors)
                                                        {
                                                            if (check(ii.AuthorName, auth.AuthorName)) flag2 = 1;
                                                        }
                                                        if (_context.Authors.Where(b => auth.AuthorName == b.AuthorName).Count() > 0)
                                                        {
                                                            flag2 = 1;
                                                        }
                                                        if (flag2 == 0) { _context.Authors.Add(auth); _context.SaveChanges(); }
                                  
                                                       
                                                    }
                                                   
                                                    AuthorsBook ab = new AuthorsBook();
                                                    ab.Book = book;
                                                    ab.Author = auth;
                                                    _context.AuthorsBooks.Add(ab);
                                                }
                                                {
                                                }


                                            }


                                        }
                                        

                                        
                                    }
                                    catch (Exception e)
                                    {
                                        ModelState.AddModelError("fileExcel", "Error");
                                    }

                                }
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }




        
    }
}
