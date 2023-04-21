using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LBApp.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;

namespace LBApp.Controllers
{
    public class ReadersBooksController : Controller
    {
        private readonly DblibraryContext _context;

        public ReadersBooksController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: ReadersBooks
        public async Task<IActionResult> Index()
        {
            var dblibraryContext = _context.ReadersBooks.Include(r => r.Book).Include(r => r.Reader);
            return View(await dblibraryContext.ToListAsync());
        }

        // GET: ReadersBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ReadersBooks == null)
            {
                return NotFound();
            }

            var readersBook = await _context.ReadersBooks
                .Include(r => r.Book)
                .Include(r => r.Reader)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (readersBook == null)
            {
                return NotFound();
            }

            return View(readersBook);
        }

        // GET: ReadersBooks/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookName");
            ViewData["ReaderId"] = new SelectList(_context.Readers, "ReaderId", "ReaderName");
            return View();
        }

        // POST: ReadersBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReaderId,BookId")] ReadersBook readersBook)
        {
            ModelState.Remove("Book");
            ModelState.Remove("Reader");
            if (ModelState.IsValid)
            {
                _context.Add(readersBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookName", readersBook.BookId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "ReaderId", "ReaderId", readersBook.ReaderId);
            return View(readersBook);
        }
        static public bool isInFav(string Rname, int BookId)
        {
            if (Rname == null) return false;
            DblibraryContext _context = new DblibraryContext();
            var t=_context.Readers.Where(c=>c.ReaderName==Rname).FirstOrDefault().ReaderId;
            var books=_context.ReadersBooks.Where(c=>c.ReaderId==t && c.BookId==BookId);
            if (books.Count() > 0) return true;
            else return false;
        }



        public IActionResult Create2(string name, int id)
        {
            ModelState.Remove("Book");
            ModelState.Remove("Reader");
            ReadersBook rb = new ReadersBook();
            //DblibraryContext _context = new DblibraryContext();
            var ReaderId = _context.Readers.Where(c => c.ReaderName == name).FirstOrDefault().ReaderId;
            rb.ReaderId = ReaderId;
            rb.BookId = id;
            if (ModelState.IsValid)
            {
                _context.Add(rb);
                 _context.SaveChangesAsync();
                return RedirectToAction("Details", "Books3", new { id = id });
            }
            //ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookName", readersBook.BookId);
            //ViewData["ReaderId"] = new SelectList(_context.Readers, "ReaderId", "ReaderId", readersBook.ReaderId);
            return View(rb);

            //ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookName");
            //ViewData["ReaderId"] = new SelectList(_context.Readers, "ReaderId", "ReaderName");
            //return View();
        }

       /* [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create2(string name, int id)
        {
            ModelState.Remove("Book");
            ModelState.Remove("Reader");
            ReadersBook rb = new ReadersBook();
            //DblibraryContext _context = new DblibraryContext();
            var ReaderId = _context.Readers.Where(c => c.ReaderName == name).FirstOrDefault().ReaderId;
            rb.ReaderId = ReaderId;
            rb.BookId = id;
           if (ModelState.IsValid)
            {
                _context.Add(rb);
                 await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Book3", new { Bookid = id });
            }
            //ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookName", readersBook.BookId);
            //ViewData["ReaderId"] = new SelectList(_context.Readers, "ReaderId", "ReaderId", readersBook.ReaderId);
            return View(rb);
            
        }*/
        // GET: ReadersBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ReadersBooks == null)
            {
                return NotFound();
            }

            var readersBook = await _context.ReadersBooks.FindAsync(id);
            if (readersBook == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookName", readersBook.BookId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "ReaderId", "ReaderName", readersBook.ReaderId);
            return View(readersBook);
        }

        // POST: ReadersBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReaderId,BookId")] ReadersBook readersBook)
        {
            if (id != readersBook.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(readersBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReadersBookExists(readersBook.Id))
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
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookName", readersBook.BookId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "ReaderId", "ReaderId", readersBook.ReaderId);
            return View(readersBook);
        }

        // GET: ReadersBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ReadersBooks == null)
            {
                return NotFound();
            }

            var readersBook = await _context.ReadersBooks
                .Include(r => r.Book)
                .Include(r => r.Reader)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (readersBook == null)
            {
                return NotFound();
            }

            return View(readersBook);
        }

        // POST: ReadersBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ReadersBooks == null)
            {
                return Problem("Entity set 'DblibraryContext.ReadersBooks'  is null.");
            }
            var readersBook = await _context.ReadersBooks.FindAsync(id);
            if (readersBook != null)
            {
                _context.ReadersBooks.Remove(readersBook);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReadersBookExists(int id)
        {
          return (_context.ReadersBooks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
