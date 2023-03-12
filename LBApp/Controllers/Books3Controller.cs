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
    public class Books3Controller : Controller
    {
        private readonly DblibraryContext _context;

        public Books3Controller(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Books3
        public async Task<IActionResult> Index()
        {

            //var dblibraryContext = _context.Books.Include(b => b.Genre).Include(b => b.PublishingHouse);
            var dblibraryContext = (from d in _context.Books select d).Include(d => d.Genre).Include(d => d.PublishingHouse).Include(n=>n.AuthorsBooks);
            //var dblibraryContext = (from d in _context.Books where d.BookId == p.BookId select d).Include(d => d.Genre).Include(d => d.PublishingHouse);

            return View(await dblibraryContext.ToListAsync());
        }

        // GET: Books3/Details/5
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

            return View(book);
        }

        // GET: Books3/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName");
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhName");
            ViewData["Authors"] = new SelectList(_context.Authors, "AuthorId", "AuthorName");
            return View();
        }

        // POST: Books3/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int[] authorBooks, [Bind("BookId,BookName,BookYear,BookPrice,GenreId,BookPagesCount,PublishingHouseId")] Book book)
        {
            ModelState.Remove("Genre");
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                foreach (var d in authorBooks) {
                    AuthorsBook ab = new AuthorsBook();
                    ab.AuthorId = d; 
                    ab.BookId = book.BookId;
                    ab.Author = _context.Authors.Where(b => ab.AuthorId == b.AuthorId).FirstOrDefault();
                    ab.Book = _context.Books.Where(b => ab.BookId == b.BookId).FirstOrDefault();
                    _context.Add(ab);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", book.GenreId);
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhName", book.PublishingHouseId);
            ViewData["Authors"] = new SelectList(_context.Authors, "AuthorId", "AuthorName");
            return View(book);
        }

        // GET: Books3/Edit/5
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
            return View(book);
        }

        // POST: Books3/Edit/5
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", book.GenreId);
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhName", book.PublishingHouseId);
            return View(book);
        }

        // GET: Books3/Delete/5
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

            return View(book);
        }

        // POST: Books3/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'DblibraryContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
    }
}
