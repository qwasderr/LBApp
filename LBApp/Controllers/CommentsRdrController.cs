using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LBApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace LBApp.Controllers
{
    public class CommentsRdrController : Controller
    {
        private readonly DblibraryContext _context;

        public CommentsRdrController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: CommentsRdr
        public async Task<IActionResult> Index(int Bookid, string Readername)
        {
            if (Bookid != 0)
            {
                ViewBag.BookName = _context.Books.Where(c => c.BookId == Bookid).FirstOrDefault().BookName;
                ViewBag.Bookid = Bookid;
                if (Readername!=null)
                ViewBag.Readerid = _context.Readers.Where(c => c.ReaderName == Readername).FirstOrDefault().ReaderId;
            }
            var dblibraryContext = _context.Comments.Where(c => c.BookId==Bookid).Include(c => c.Reader);
            return View(await dblibraryContext.ToListAsync());
        }

        // GET: CommentsRdr/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }
            ViewBag.Bookid = _context.Comments.Where(c => c.ComId == id).FirstOrDefault().BookId;
            var comment = await _context.Comments
                .Include(c => c.Book)
                .Include(c => c.Reader)
                .FirstOrDefaultAsync(m => m.ComId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: CommentsRdr/Create
        [Authorize(Roles = "user, admin")]
        public IActionResult Create(int Bookid, int Readerid)
        {
            ViewBag.Bookid = Bookid;
            //ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookName");
            //ViewData["ReaderId"] = new SelectList(_context.Readers, "ReaderId", "ReaderName");
            ViewBag.Readerid= Readerid;
            return View();
        }

        // POST: CommentsRdr/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int Bookid, [Bind("ComId,ComText,ComDate,BookId,ReaderId")] Comment comment)
        {
            ViewBag.Bookid = Bookid;
            var rid = _context.Readers.Where(c => c.ReaderId == comment.ReaderId).FirstOrDefault().ReaderName;
            ModelState.Remove("Book");
            ModelState.Remove("Reader");
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { Bookid = Bookid, Readername = rid });
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookName", comment.BookId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "ReaderId", "ReaderName", comment.ReaderId);
            return View(comment);
        }

        // GET: CommentsRdr/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }
            ViewBag.Bookid = _context.Comments.Where(c => c.ComId == id).FirstOrDefault().BookId;
            ViewBag.ComDate= _context.Comments.Where(c => c.ComId == id).FirstOrDefault().ComDate;
            ViewBag.ReaderId = _context.Comments.Where(c => c.ComId == id).FirstOrDefault().ReaderId;
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            //ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookName", comment.BookId);
            //ViewData["ReaderId"] = new SelectList(_context.Readers, "ReaderId", "ReaderName", comment.ReaderId);
            return View(comment);
        }

        // POST: CommentsRdr/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ComId,ComText,ComDate,BookId,ReaderId")] Comment comment)
        {
            if (id != comment.ComId)
            {
                return NotFound();
            }
            ViewBag.Bookid = comment.BookId;
            var rid=_context.Readers.Where(c=>c.ReaderId==comment.ReaderId).FirstOrDefault().ReaderName;
            ModelState.Remove("Book");
            ModelState.Remove("Reader");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.ComId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { Bookid = comment.BookId, Readername = rid });
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookName", comment.BookId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "ReaderId", "ReaderName", comment.ReaderId);
            return View(comment);
        }

        // GET: CommentsRdr/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }
            ViewBag.Bookid = _context.Comments.Where(c => c.ComId == id).FirstOrDefault().BookId;
            var comment = await _context.Comments
                .Include(c => c.Book)
                .Include(c => c.Reader)
                .FirstOrDefaultAsync(m => m.ComId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: CommentsRdr/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comments == null)
            {
                return Problem("Entity set 'DblibraryContext.Comments'  is null.");
            }
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }
            ViewBag.Bookid = _context.Comments.Where(c => c.ComId == id).FirstOrDefault().BookId;
            var r=  _context.Comments.Where(c => c.ComId == id).FirstOrDefault().ReaderId;
            var rid = _context.Readers.Where(c => c.ReaderId == r).FirstOrDefault().ReaderName;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { Bookid = ViewBag.Bookid, Readername = rid });
        }

        private bool CommentExists(int id)
        {
          return (_context.Comments?.Any(e => e.ComId == id)).GetValueOrDefault();
        }
    }
}
