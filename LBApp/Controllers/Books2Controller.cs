using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LBApp.Models;

namespace LBApp.Controllers
{
    public class Books2Controller : Controller
    {
        private readonly DblibraryContext _context;

        public Books2Controller(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Books2
        public async Task<IActionResult> Index(int? id)
        {
            ViewBag.ReaderId = id;
            //var dblibraryContext = _context.Books.Include(b => b.Genre).Include(b => b.PublishingHouse);
            var dblibraryContext = (from p in _context.ReadersBooks where (p.ReaderId == id) from d in _context.Books where d.BookId == p.BookId select d).Include(d => d.Genre).Include(d => d.PublishingHouse);

            return View(await dblibraryContext.ToListAsync());
        }

        // GET: Books2/Details/5
        public async Task<IActionResult> Details(int? id, int readerId)
        {
            ViewBag.ReaderId = readerId;
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

            return View(book);
        }

        // GET: Books2/Create
        public IActionResult Create(int readerId)
        {
            ViewBag.ReaderId = readerId;
            //ViewBag.ReaderName = _context.ReadersBooks.Where(b => b.ReaderId == readerId).FirstOrDefault().ReaderName;
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName");
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhName");
            return View();
        }

        // POST: Books2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int readerId, [Bind("BookId,BookName,BookYear,BookPrice,GenreId,BookPagesCount,PublishingHouseId")] Book book)
        {
            ViewBag.ReaderId = readerId;
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
                _context.Add(book);
                await _context.SaveChangesAsync();
                ReadersBook rb = new ReadersBook();
                rb.ReaderId = readerId;
                rb.BookId = book.BookId;
                _context.Add(rb);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Books2", new { id = readerId});

            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", book.GenreId);
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhName", book.PublishingHouseId);
            return View(book);
        }

        // GET: Books2/Edit/5
        public async Task<IActionResult> Edit(int? id, int readerId)
        {
            ViewBag.ReaderId = readerId;
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
            return View(book);
        }

        // POST: Books2/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int readerId, [Bind("BookId,BookName,BookYear,BookPrice,GenreId,BookPagesCount,PublishingHouseId")] Book book)
        {
            ViewBag.ReaderId = readerId;
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
                return RedirectToAction("Index", "Books2", new { id = readerId});

            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", book.GenreId);
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhName", book.PublishingHouseId);
            return View(book);
        }

        // GET: Books2/Delete/5
        public async Task<IActionResult> Delete(int? id, int readerId)
        {
            ViewBag.ReaderId = readerId;
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

            return View(book);
        }

        // POST: Books2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int readerId)
        {
            ViewBag.ReaderId = readerId;
            if (_context.Books == null)
            {
                return Problem("Entity set 'DblibraryContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            var t = _context.ReadersBooks.Where(b => b.BookId == id);
            if (book != null)
            {
                foreach (var q in t)
                {
                    var idab = q.Id;
                    var ab = await _context.ReadersBooks.FindAsync(idab);
                    _context.ReadersBooks.Remove(ab);
                    //await _context.SaveChangesAsync();
                }
                _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            //_context.Dispose();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Books2", new { id = ViewBag.ReaderId});
        }

        private bool BookExists(int id)
        {
          return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
    }
}
