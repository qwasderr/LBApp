using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LBApp.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace LBApp.Controllers
{
    public class Books1Controller : Controller
    {
        private readonly DblibraryContext _context;

        public Books1Controller(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Books1
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if(id == null) return RedirectToAction("Index", "Authors");
            ViewBag.AuthorId = id;
            ViewBag.AuthorName = name;
            //var dblibraryContext = _context.Books.Include(b => b.Genre).Include(b => b.PublishingHouse);

            //var dblibraryContext = _context.Books.Where(b=>b.BookId == foreach (var a in _context.AuthorsBooks.Where(c=>c.AuthorId==id).Include(l=>l.BookId)) ).Include(b=>b.Genre).Include(b=>b.PublishingHouse);

            /*var dblibraryContext = _context.Books.(x in _context.AuthorsBooks.Where(c => c.AuthorId == id).Include(l => l.BookId));

            foreach (var a in _context.AuthorsBooks.Where(c => c.AuthorId == id))
            {
                //var d = _context.Books.Where(b => b.BookId == a.BookId ).Include(b => b.Genre).Include(b => b.PublishingHouse);
                //b.Add(d);
                dd.Include(_context.Books.Where(b => b.BookId == a.BookId).Include(b => b.Genre).Include(b => b.PublishingHouse));
            }*/
            var dblibraryContext = (from p in _context.AuthorsBooks where (p.AuthorId == id) from d in _context.Books where d.BookId == p.BookId select d).Include(d=>d.Genre).Include(d => d.PublishingHouse);

            return View(await dblibraryContext.ToListAsync());
            //return View(await dd.ToListAsync());
        }

        // GET: Books1/Details/5
        public async Task<IActionResult> Details(int? id, int authorId)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }
            ViewBag.AuthorId = id;
            var book = await _context.Books
                .Include(b => b.Genre)
                .Include(b => b.PublishingHouse)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.AuthorId = _context.AuthorsBooks.Where(b => b.BookId == id).FirstOrDefault().AuthorId;
            var idd = _context.AuthorsBooks.Where(b => b.BookId == id).FirstOrDefault().AuthorId;
            ViewBag.AuthorName = _context.Authors.Where(b => b.AuthorId == idd).FirstOrDefault().AuthorName;
            return View(book);
        }

        // GET: Books1/Create
        public IActionResult Create(int authorId)
        {
            ViewBag.AuthorId = authorId;
            ViewBag.AuthorName = _context.Authors.Where(b => b.AuthorId == authorId).FirstOrDefault().AuthorName;
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName");
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhName");
            return View();
        }

        // POST: Books1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int authorId, [Bind("BookId,BookName,BookYear,BookPrice,GenreId,BookPagesCount,PublishingHouseId")] Book book)
        {
            
            ModelState.Remove("Genre");
            ViewBag.AuthorId = authorId;
            ViewBag.AuthorName = _context.Authors.Where(b => b.AuthorId == authorId).FirstOrDefault().AuthorName;
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
                
                _context.Add(book);
                await _context.SaveChangesAsync();
                AuthorsBook ab = new AuthorsBook();
                ab.AuthorId = authorId;
                ab.BookId = book.BookId;
                _context.Add(ab);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Books1", new { id = authorId, name = _context.Authors.Where(b => b.AuthorId == authorId).FirstOrDefault().AuthorName });
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", book.GenreId);
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhName", book.PublishingHouseId);
            return View(book);
            //return RedirectToAction("Index", "Books1", new { id = authorId, name = _context.Authors.Where(b => b.AuthorId == authorId).FirstOrDefault().AuthorName });

        }

        // GET: Books1/Edit/5
        public async Task<IActionResult> Edit(int? id, int authorId)
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
            //ViewBag.AuthorId = _context.AuthorsBooks.Where(b => b.BookId == id).FirstOrDefault().AuthorId;
            ViewBag.AuthorId = authorId;
            //var idd = _context.AuthorsBooks.Where(b => b.BookId == id).FirstOrDefault().AuthorId;
            ViewBag.AuthorName = _context.Authors.Where(b => b.AuthorId == authorId).FirstOrDefault().AuthorName;
            //ViewBag.BookName = _context.Books.Where(b => b.BookId == id).FirstOrDefault().BookName;
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", book.GenreId);
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhName", book.PublishingHouseId);
            return View(book);
        }

        // POST: Books1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int authorId, [Bind("BookId,BookName,BookYear,BookPrice,GenreId,BookPagesCount,PublishingHouseId")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }
            ViewBag.AuthorId = authorId;
            var idd = _context.AuthorsBooks.Where(b => b.BookId == id).FirstOrDefault().AuthorId;
            ViewBag.AuthorName = _context.Authors.Where(b => b.AuthorId == authorId).FirstOrDefault().AuthorName;
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
            if (book.BookPagesCount < 0)
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
                return RedirectToAction("Index", "Books1", new { id = authorId, name = _context.Authors.Where(b => b.AuthorId == authorId).FirstOrDefault().AuthorName });
            }
            
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", book.GenreId);
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhName", book.PublishingHouseId);
            return View(book);
        }

        // GET: Books1/Delete/5
        public async Task<IActionResult> Delete(int? id, int authorId)
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
            ViewBag.AuthorId = authorId;
            var idd = _context.AuthorsBooks.Where(b => b.BookId == id).FirstOrDefault().AuthorId;
            ViewBag.AuthorName = _context.Authors.Where(b => b.AuthorId == authorId).FirstOrDefault().AuthorName;
            //ViewBag.AuthorId = _context.AuthorsBooks.Where(b=>b.BookId==id).FirstOrDefault().AuthorId;
            //var idd= _context.AuthorsBooks.Where(b => b.BookId == id).FirstOrDefault().AuthorId;
            //ViewBag.AuthorName = _context.Authors.Where(b => b.AuthorId == idd).FirstOrDefault().AuthorName;
            return View(book);
        }

        // POST: Books1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int authorId)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'DblibraryContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            //var idab = _context.AuthorsBooks.Where(b=>b.BookId==id).FirstOrDefault().Id;
            //var ab = await _context.AuthorsBooks.FindAsync(idab);
            if (book != null)
            {
                //_context.AuthorsBooks.Remove(ab);
                foreach (var q in _context.AuthorsBooks.Where(b => b.BookId == id))
                {
                    var idab = q.Id;
                    var ab = await _context.AuthorsBooks.FindAsync(idab);
                    _context.AuthorsBooks.Remove(ab);
                }
                _context.Books.Remove(book);
                
            }
            //ViewBag.AuthorId = _context.AuthorsBooks.Where(b => b.BookId == id).FirstOrDefault().AuthorId;
            //var idd = _context.AuthorsBooks.Where(b => b.BookId == id).FirstOrDefault().AuthorId;
            //ViewBag.AuthorName = _context.Authors.Where(b => b.AuthorId == idd).FirstOrDefault().AuthorName;
            ViewBag.AuthorId = authorId;
            var idd = _context.AuthorsBooks.Where(b => b.BookId == id).FirstOrDefault().AuthorId;
            ViewBag.AuthorName = _context.Authors.Where(b => b.AuthorId == authorId).FirstOrDefault().AuthorName;
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Books1", new { id = ViewBag.AuthorId, name = ViewBag.AuthorName });
        }

        private bool BookExists(int id)
        {
          return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
    }
}
