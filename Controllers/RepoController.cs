using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FileGoat.Data;
using FileGoat.Models;
using Microsoft.AspNetCore.Authorization;

namespace FileGoat.Controllers
{
    [Authorize]
    public class RepoController : Controller
    {
        private readonly FileGoatContext _context;

        public RepoController(FileGoatContext context)
        {
            _context = context;
        }

        // GET: Repo
        public async Task<IActionResult> Index()
        {
            return _context.Repo != null ?
                        View(await _context.Repo.ToListAsync()) :
                        Problem("Entity set 'FileGoatContext.Repo'  is null.");
        }

        // GET: Repo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Repo == null)
            {
                return NotFound();
            }

            var repo = await _context.Repo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repo == null)
            {
                return NotFound();
            }

            return View(repo);
        }

        // GET: Repo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Repo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Repo repo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(repo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(repo);
        }

        // GET: Repo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Repo == null)
            {
                return NotFound();
            }

            var repo = await _context.Repo.FindAsync(id);
            if (repo == null)
            {
                return NotFound();
            }
            return View(repo);
        }

        // POST: Repo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Repo repo)
        {
            if (id != repo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepoExists(repo.Id))
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
            return View(repo);
        }

        // GET: Repo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Repo == null)
            {
                return NotFound();
            }

            var repo = await _context.Repo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repo == null)
            {
                return NotFound();
            }

            return View(repo);
        }

        // POST: Repo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Repo == null)
            {
                return Problem("Entity set 'FileGoatContext.Repo'  is null.");
            }
            var repo = await _context.Repo.FindAsync(id);
            if (repo != null)
            {
                _context.Repo.Remove(repo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepoExists(int id)
        {
            return (_context.Repo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
