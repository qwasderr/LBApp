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
    public class ReadersController : Controller
    {
        private readonly DblibraryContext _context;

        public ReadersController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Readers
        public async Task<IActionResult> Index()
        {
              return _context.Readers != null ? 
                          View(await _context.Readers.ToListAsync()) :
                          Problem("Entity set 'DblibraryContext.Readers'  is null.");
        }

        // GET: Readers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Readers == null)
            {
                return NotFound();
            }

            var reader = await _context.Readers
                .FirstOrDefaultAsync(m => m.ReaderId == id);
            if (reader == null)
            {
                return NotFound();
            }

            //return View(reader);
            return RedirectToAction("Index", "Books2", new { id = reader.ReaderId, name = reader.ReaderName });
        }

        // GET: Readers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Readers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReaderId,ReaderName")] Reader reader)
        {
            if (_context.Readers.Where(b => reader.ReaderName == b.ReaderName).Count() > 0)
            {
                ModelState.AddModelError("ReaderName", "Користувач з таким ім'ям вже існує");
            }
            if (ModelState.IsValid)
            {
                _context.Add(reader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reader);
        }

        // GET: Readers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Readers == null)
            {
                return NotFound();
            }

            var reader = await _context.Readers.FindAsync(id);
            if (reader == null)
            {
                return NotFound();
            }
            return View(reader);
        }

        // POST: Readers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReaderId,ReaderName")] Reader reader)
        {
            if (id != reader.ReaderId)
            {
                return NotFound();
            }
            var namee = _context.Readers.Where(b => reader.ReaderName == b.ReaderName);
            if (namee.Count() > 0 && namee.Where(b => b.ReaderId == id).Count() == 0)
            {
                ModelState.AddModelError("ReaderName", "Користувач з таким ім'ям вже існує");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReaderExists(reader.ReaderId))
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
            return View(reader);
        }

        // GET: Readers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Readers == null)
            {
                return NotFound();
            }

            var reader = await _context.Readers
                .FirstOrDefaultAsync(m => m.ReaderId == id);
            if (reader == null)
            {
                return NotFound();
            }

            return View(reader);
        }

        // POST: Readers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Readers == null)
            {
                return Problem("Entity set 'DblibraryContext.Readers'  is null.");
            }
            var reader = await _context.Readers.FindAsync(id);
            var t = _context.ReadersBooks.Where(b => b.ReaderId == id);
            if (reader != null)
            {
                foreach (var q in t.ToList())
                {
                    var idab2 = q.BookId;
                    //var ab =  _context.ReadersBooks.Find(idab2);
                    var ab = _context.ReadersBooks.Where(b=>b.BookId==idab2).FirstOrDefault();
                    _context.ReadersBooks.Remove(ab);
                    //await _context.SaveChangesAsync();
                }
                foreach (var q in _context.Comments.Where(b=>b.ReaderId==id))
                {
                    //var ab =  _context.ReadersBooks.Find(idab2);
                    //var ab = _context.ReadersBooks.Where(b => b.BookId == idab2).FirstOrDefault();
                    _context.Comments.Remove(q);
                    //await _context.SaveChangesAsync();
                }
                _context.Readers.Remove(reader);
            }
            
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private bool ReaderExists(int id)
        {
          return (_context.Readers?.Any(e => e.ReaderId == id)).GetValueOrDefault();
        }
    }
}
