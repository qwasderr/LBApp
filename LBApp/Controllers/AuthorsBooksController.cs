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
    public class AuthorsBooksController : Controller
    {
        private readonly DblibraryContext _context;

        public AuthorsBooksController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: AuthorsBooks
        public async Task<IActionResult> Index()
        {
            var dblibraryContext = _context.AuthorsBooks.Include(a => a.Author).Include(a => a.Book);
            return View(await dblibraryContext.ToListAsync());
        }

        // GET: AuthorsBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AuthorsBooks == null)
            {
                return NotFound();
            }

            var authorsBook = await _context.AuthorsBooks
                .Include(a => a.Author)
                .Include(a => a.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authorsBook == null)
            {
                return NotFound();
            }

            return View(authorsBook);
        }

        // GET: AuthorsBooks/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorName");
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookName");
            return View();
        }

        // POST: AuthorsBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AuthorId,BookId")] AuthorsBook authorsBook)
        {
            ModelState.Remove("Book");
            ModelState.Remove("Author");
            authorsBook.Author = _context.Authors.Where(b=>authorsBook.AuthorId==b.AuthorId).FirstOrDefault();
            authorsBook.Book = _context.Books.Where(b => authorsBook.BookId == b.BookId).FirstOrDefault();
            if (_context.AuthorsBooks.Where(b=>authorsBook.AuthorId==b.AuthorId).Where(b=> authorsBook.BookId == b.BookId).Count()>0)
            {
                ModelState.AddModelError("AuthorId", "Дублікат");
            }

            if (ModelState.IsValid)
            {
                _context.Add(authorsBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", authorsBook.AuthorId);
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorName", authorsBook.AuthorId);
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookName", authorsBook.BookId);
            return View(authorsBook);
        }

        // GET: AuthorsBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AuthorsBooks == null)
            {
                return NotFound();
            }

            var authorsBook = await _context.AuthorsBooks.FindAsync(id);
            if (authorsBook == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorName", authorsBook.AuthorId);
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookName", authorsBook.BookId);
            return View(authorsBook);
        }

        // POST: AuthorsBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AuthorId,BookId")] AuthorsBook authorsBook)
        {
            if (id != authorsBook.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Book");
            ModelState.Remove("Author");
            authorsBook.Author = _context.Authors.Where(b => authorsBook.AuthorId == b.AuthorId).FirstOrDefault();
            authorsBook.Book = _context.Books.Where(b => authorsBook.BookId == b.BookId).FirstOrDefault();
            if (_context.AuthorsBooks.Where(b => authorsBook.AuthorId == b.AuthorId).Where(b => authorsBook.BookId == b.BookId).Count() > 0)
            {
                ModelState.AddModelError("AuthorId", "Дублікат");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(authorsBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorsBookExists(authorsBook.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorName", authorsBook.AuthorId);
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookName", authorsBook.BookId);
            return View(authorsBook);
        }

        // GET: AuthorsBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AuthorsBooks == null)
            {
                return NotFound();
            }

            var authorsBook = await _context.AuthorsBooks
                .Include(a => a.Author)
                .Include(a => a.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authorsBook == null)
            {
                return NotFound();
            }

            return View(authorsBook);
        }

        // POST: AuthorsBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AuthorsBooks == null)
            {
                return Problem("Entity set 'DblibraryContext.AuthorsBooks'  is null.");
            }
            var authorsBook = await _context.AuthorsBooks.FindAsync(id);
            if (authorsBook != null)
            {
                _context.AuthorsBooks.Remove(authorsBook);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorsBookExists(int id)
        {
          return (_context.AuthorsBooks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
