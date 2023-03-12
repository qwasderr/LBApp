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
    public class AuthorsController : Controller
    {
        private readonly DblibraryContext _context;

        public AuthorsController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
              return _context.Authors != null ? 
                          View(await _context.Authors.ToListAsync()) :
                          Problem("Entity set 'DblibraryContext.Authors'  is null.");
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }

            //return View(author);
            return RedirectToAction("Index", "Books1", new { id = author.AuthorId, name = author.AuthorName });
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuthorId,AuthorName,AuthorDate,AuthorBiogr")] Author author)
        {
            if (_context.Authors.Where(b => author.AuthorName == b.AuthorName).Count() > 0)
            {
                ModelState.AddModelError("AuthorName", "Автор з таким ім'ям вже існує");
            }
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,AuthorName,AuthorDate,AuthorBiogr")] Author author)
        {
            if (id != author.AuthorId)
            {
                return NotFound();
            }
            /*if (_context.Authors.Where(b => author.AuthorName == b.AuthorName).Count() > 0)
            {
                ModelState.AddModelError("AuthorName", "Автор з таким ім'ям вже існує");
            }*/
            var namee = _context.Authors.Where(b => author.AuthorName == b.AuthorName);
            if (namee.Count() > 0 && namee.Where(b => b.AuthorId == id).Count() == 0)
            {
                ModelState.AddModelError("AuthorName", "Автор з таким ім'ям вже існує");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.AuthorId))
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
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Authors == null)
            {
                return Problem("Entity set 'DblibraryContext.Authors'  is null.");
            }
            var author = await _context.Authors.FindAsync(id);
            if (_context.AuthorsBooks.Where(b => b.AuthorId == author.AuthorId).Count() > 0)
            {
                //return Problem("У цього автора є книги");
                ModelState.AddModelError("AuthorId", "У цього автора є книги");
                return View(author);
            }
            if (author != null)
            {
                _context.Authors.Remove(author);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
          return (_context.Authors?.Any(e => e.AuthorId == id)).GetValueOrDefault();
        }
    }
}
