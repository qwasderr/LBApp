using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LBApp.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LBApp.Controllers
{
    public class Books4Controller : Controller
    {
        private readonly DblibraryContext _context;

        public Books4Controller(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Books4
        public async Task<IActionResult> Index(string name)
        {

            var t = _context.Readers.Where(c => c.ReaderName == name).FirstOrDefault().ReaderId;
            ViewBag.ReaderId = t;
            //var dblibraryContext = _context.Books.Include(b => b.Genre).Include(b => b.PublishingHouse);
            //return View(await dblibraryContext.ToListAsync());
            var dblibraryContext = (from p in _context.ReadersBooks where (p.ReaderId == t) from d in _context.Books where d.BookId == p.BookId select d).Include(d => d.Genre).Include(d => d.PublishingHouse);

            return View(await dblibraryContext.ToListAsync());
        }

        // GET: Books4/Details/5
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

        // GET: Books4/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName");
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhId");
            return View();
        }

        // POST: Books4/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,BookName,BookYear,BookPrice,GenreId,BookPagesCount,PublishingHouseId")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenreId", "GenreName", book.GenreId);
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhId", book.PublishingHouseId);
            return View(book);
        }

        // GET: Books4/Edit/5
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
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhId", book.PublishingHouseId);
            return View(book);
        }

        // POST: Books4/Edit/5
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
            ViewData["PublishingHouseId"] = new SelectList(_context.PublishingHouses, "PhId", "PhId", book.PublishingHouseId);
            return View(book);
        }

        // GET: Books4/Delete/5
        public async Task<IActionResult> Delete(int? id, string name)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }
            ViewBag.ReaderName = name;
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

        // POST: Books4/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string name)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'DblibraryContext.Books'  is null.");
            }
            var rid= _context.Readers.Where(c => c.ReaderName == name).FirstOrDefault().ReaderId;
            ViewBag.ReaderId = rid;
            //var book = await _context.Books.FindAsync(id);
            var books = _context.ReadersBooks.Where(c => c.ReaderId == rid && c.BookId == id).ToList();
            if (books != null)
            {
                foreach (var b in books)
                {
                    _context.ReadersBooks.Remove(b);
                    _context.SaveChanges();
                }
                
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Books4", new { name = name });
        }

        private bool BookExists(int id)
        {
          return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
        public static bool HasBooks(string name)
        {
            DblibraryContext _context = new DblibraryContext();
            var rid = _context.Readers.Where(c => c.ReaderName == name).FirstOrDefault().ReaderId;
            var b = _context.ReadersBooks.Where(c => c.ReaderId == rid).ToList();
            if (b.Count() > 0) return true;
            else return false;
        }
        public IActionResult Buy(string name)
        {
            var rid = _context.Readers.Where(c => c.ReaderName == name).FirstOrDefault().ReaderId;
            var b=_context.ReadersBooks.Where(c=>c.ReaderId==rid).ToList();
            foreach(var book in b)
            {
                _context.ReadersBooks.Remove(book);
                _context.SaveChanges();
            }
            return View();
        }
    }
}
