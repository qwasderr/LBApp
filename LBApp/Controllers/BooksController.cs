using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LBApp.Models;
using ClosedXML.Excel;
using static System.Reflection.Metadata.BlobBuilder;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace LBApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly DblibraryContext _context;

        public BooksController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Index", "Genres");
            ViewBag.GenreId = id;
            ViewBag.GenreName = name;
            //var dblibraryContext = _context.Books.Include(b => b.Genre).Include(b => b.PublishingHouse);
            var dblibraryContext = _context.Books.Where(b => b.GenreId == id).Include(b => b.Genre).Include(b => b.PublishingHouse);
            return View(await dblibraryContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Genre)
                .Include(b => b.PublishingHouse)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.GenreId = book.GenreId;
            ViewBag.GenreName = _context.Genres.Where(b => b.GenreId == book.GenreId).FirstOrDefault().GenreName;
            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create(int genreId)
        {
            //ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName");
            ViewBag.GenreId = genreId;
            ViewBag.GenreName = _context.Genres.Where(b => b.GenreId == genreId).FirstOrDefault().GenreName;
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int genreId, [Bind("BookId,BookName,BookYear,BookPrice,BookPagesCount,PublishingHouseId")] Book book)
        {
            book.GenreId = genreId;

            ModelState.Remove("Genre");
            if (_context.Books.Where(b => book.BookName == b.BookName).Count() > 0)
            {
                ModelState.AddModelError("BookName", "Книжка з таким ім'ям вже існує");
            }
            if (book.BookYear < 0 || book.BookYear > DateTime.Now.Year)
            {
                ModelState.AddModelError("BookYear", "Неможливий рік");
            }
            if (book.BookPrice < 0)
            {
                ModelState.AddModelError("BookPrice", "Неможлuва ціна");
            }
            if (book.BookPagesCount < 0)
            {
                ModelState.AddModelError("BookPagesCount", "Неможлuва кількість сторінок");
            }
            if (ModelState.IsValid)
            {
                Console.WriteLine(book.BookId);
                _context.Add(book);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Books", new { id = book.GenreId, name = _context.Genres.Where(b => b.GenreId == genreId).FirstOrDefault().GenreName });
            }
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhName", book.PublishingHouseId);
            //ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", book.GenreId);
            ViewBag.GenreId = genreId;
            ViewBag.GenreName = _context.Genres.Where(b => b.GenreId == genreId).FirstOrDefault().GenreName;
            //return View(book);
            //return RedirectToAction("Index", "Books", new {id=book.GenreId, name=_context.Genres.Where(b=>b.GenreId==genreId).FirstOrDefault().GenreName});
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", book.GenreId);
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhName", book.PublishingHouseId);
            ViewBag.GenreIdd = book.GenreId;
            ViewBag.GenreName = _context.Genres.Where(b => b.GenreId == book.GenreId).FirstOrDefault().GenreName;
            //ViewBag.BookName = _context.Books.Where(b => b.BookId == id).FirstOrDefault().BookName;
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,BookName,BookYear,BookPrice,GenreId,BookPagesCount,PublishingHouseId")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            ModelState.Remove("Genre");
            var namee = _context.Books.Where(b => book.BookName == b.BookName);
            if (namee.Count() > 0 && namee.Where(b => b.BookId == id).Count() == 0)
            {
                ModelState.AddModelError("BookName", "Книжка з таким ім'ям вже існує");
            }
            if (book.BookYear < 0 || book.BookYear > DateTime.Now.Year)
            {
                ModelState.AddModelError("BookYear", "Неможливий рік");
            }
            if (book.BookPrice < 0)
            {
                ModelState.AddModelError("BookPrice", "Неможлuва ціна");
            }
            if (book.BookPagesCount <= 0)
            {
                ModelState.AddModelError("BookPagesCount", "Неможлuва кількість сторінок");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Books", new { id = book.GenreId, name = _context.Genres.Where(b => b.GenreId == book.GenreId).FirstOrDefault().GenreName });
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", book.GenreId);
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhName", book.PublishingHouseId);
            ViewBag.GenreIdd = book.GenreId;
            ViewBag.GenreName = _context.Genres.Where(b => b.GenreId == book.GenreId).FirstOrDefault().GenreName;
            return View(book);
            //return RedirectToAction("Index", "Books", new { id = book.GenreId, name = _context.Genres.Where(b => b.GenreId == book.GenreId).FirstOrDefault().GenreName });
        }

        // GET: Books/Delete/5
        public IActionResult Bags(int? id)
        {
            //ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName");
            ViewBag.GenreId = id;
            ViewBag.GenreName = _context.Genres.Where(b => b.GenreId == id).FirstOrDefault().GenreName;
            //ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhId");
            return View();
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Genre)
                .Include(b => b.PublishingHouse)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.GenreId = book.GenreId;
            ViewBag.GenreName = _context.Genres.Where(b => b.GenreId == book.GenreId).FirstOrDefault().GenreName;
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'DblibraryContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            //var idab = _context.AuthorsBooks.Where(b => b.BookId == id).FirstOrDefault().Id;
            //var ab = await _context.AuthorsBooks.FindAsync(idab);
            if (book != null)
            {
                foreach (var q in _context.AuthorsBooks.Where(b => b.BookId == id))
                {
                    var idab = q.Id;
                    var ab = await _context.AuthorsBooks.FindAsync(idab);
                    _context.AuthorsBooks.Remove(ab);
                }
                _context.Books.Remove(book);
            }
            ViewBag.GenreId = book.GenreId;
            ViewBag.GenreName = _context.Genres.Where(b => b.GenreId == book.GenreId).FirstOrDefault().GenreName;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Books", new { id = ViewBag.GenreId, name = ViewBag.GenreName });
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        }





        public ActionResult Export(int genreId)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var gname = _context.Genres.Where(b => b.GenreId == genreId).FirstOrDefault().GenreName;
                var worksheet = workbook.Worksheets.Add(gname);
                worksheet.Cell("A1").Value = "Назва";
                worksheet.Cell("B1").Value = "Кількість сторінок";
                worksheet.Cell("C1").Value = "Ціна";
                worksheet.Cell("D1").Value = "Рік написання";
                worksheet.Cell("E1").Value = "Автор 1";
                worksheet.Cell("F1").Value = "Автор 2";
                worksheet.Cell("G1").Value = "Автор 3";
                worksheet.Row(1).Style.Font.Bold = true;
                var books = _context.Books.Where(b => b.GenreId == genreId).ToList();
                int temp = 2;
                for (int i = 0; i < books.Count; i++)
                {
                    worksheet.Cell(i + temp, 1).Value = books[i].BookName;
                    worksheet.Cell(i + temp, 2).Value = books[i].BookPagesCount;
                    worksheet.Cell(i + temp, 3).Value = books[i].BookPrice;
                    worksheet.Cell(i + temp, 4).Value = books[i].BookYear;
                    var ab = _context.AuthorsBooks.Where(a => a.BookId == books[i].BookId).Include("Author").ToList();
                    int j = 0;
                    int maxauth = 3;
                    int temp2 = 5;
                    foreach (var a in ab)
                    {
                        if (j <= maxauth)
                        {
                            worksheet.Cell(i + temp, j + temp2).Value = a.Author.AuthorName;
                            j++;
                        }
                    }

                }



                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {

                        FileDownloadName = $"Books_Genres_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }





            }




        }
    }
}
