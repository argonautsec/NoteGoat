using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteGoat.Data;
using NoteGoat.Models;
using Microsoft.AspNetCore.Authorization;
using NoteGoat.Areas.Identity.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NoteGoat;

[Authorize(Roles = Role.User)]
public class NoteController : Controller
{
    private readonly NoteGoatContext _context;
    private readonly ILogger<NoteController> _logger;

    public NoteController(NoteGoatContext context, ILogger<NoteController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Note
    public async Task<IActionResult> Index(string? q)
    {
        _logger.LogInformation("Query string = {}", q);

        var user = await _context.User.FirstAsync(u => u.UserName == User.Identity.Name);
        var assignedRepos = await _context.Repo
                        .Include(r => r.Users)
                        .Where(r => r.Users.Contains(user))
                        .Select(r => r.Id)
                        .ToListAsync();

        ViewBag.Q = q;
        return _context.Note != null ?
                    View(await _context.Note
                    .Include(n => n.Attachment)
                    .Include(r => r.Repo)
                    .Where(n => assignedRepos.Contains(n.RepoId))
                    .Where(n => n.Content.Contains(q) || n.Title.Contains(q) || q == null)
                    .ToListAsync()) :
                    Problem("Entity set 'NoteGoatContext.Note'  is null.");
    }

    // GET: Note/Create
    public async Task<IActionResult> Create()
    {
        var user = await _context.User.FirstAsync(u => u.UserName == User.Identity.Name);
        var assignedRepos = await _context.Repo
                        .Include(r => r.Users)
                        .Where(r => r.Users.Contains(user)).ToListAsync();

        ViewBag.Repos = assignedRepos.Select(i => new SelectListItem
        {
            Text = i.Name,
            Value = i.Id.ToString()
        }).ToList();

        return View();
    }


    // GET: Note/DownloadAttachment/4
    public async Task<IActionResult> DownloadAttachment(int? id)
    {
        if (id == null || _context.Attachment == null)
        {
            return NotFound();
        }

        var attachment = await _context.Attachment.FindAsync(id);
        if (attachment == null)
        {
            return NotFound();
        }

        return File(attachment.Content, attachment.ContentType, attachment.Name);
    }

    // POST: Note/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        IFormFile? formFile,
        [Bind("Id,Title,Content,Created,RepoId")] Note note)
    {
        if (!ModelState.IsValid)
        {
            return View(note);
        }

        var user = await _context.User.FirstAsync(u => u.UserName == User.Identity.Name);
        var assignedRepos = await _context.Repo
                        .Include(r => r.Users)
                        .Where(r => r.Users.Contains(user))
                        .Select(r => r.Id)
                        .ToListAsync();

        if (!assignedRepos.Contains(note.RepoId))
        {
            return Forbid();
        }

        if (formFile != null)
        {
            note.Attachment = await Attachment.FromFormFileAsync(formFile);
        }

        _logger.LogInformation("Creating note {}", note);
        _context.Add(note);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Note/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Note == null)
        {
            return NotFound();
        }

        var note = await _context.Note.FindAsync(id);
        if (note == null)
        {
            return NotFound();
        }

        var user = await _context.User.FirstAsync(u => u.UserName == User.Identity.Name);
        var assignedRepos = await _context.Repo
                        .Include(r => r.Users)
                        .Where(r => r.Users.Contains(user)).ToListAsync();

        ViewBag.Repos = assignedRepos.Select(i => new SelectListItem
        {
            Text = i.Name,
            Value = i.Id.ToString()
        }).ToList();

        _logger.LogInformation("Loading note {}", note);
        return View(note);
    }

    // POST: Note/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        IFormFile? formFile,
        [Bind("Id,Title,Content,Created,FileName,RepoId")] Note note)
    {
        if (id != note.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(note);
        }

        var user = await _context.User.FirstAsync(u => u.UserName == User.Identity.Name);
        var assignedRepos = await _context.Repo
                        .Include(r => r.Users)
                        .Where(r => r.Users.Contains(user))
                        .Select(r => r.Id)
                        .ToListAsync();

        if (!assignedRepos.Contains(note.RepoId))
        {
            return Forbid();
        }

        if (formFile != null)
        {
            var oldAttachment = _context.Attachment.FirstOrDefault(a => a.NoteId == id);
            if (oldAttachment != null)
            {
                _context.Attachment.Remove(oldAttachment);
            }
            var attachment = await Attachment.FromFormFileAsync(formFile);
            attachment.NoteId = id;
            _context.Add(attachment);

            await _context.SaveChangesAsync();

            note.Attachment = attachment;
        }

        try
        {
            _context.Update(note);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Saved note {}", note);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!NoteExists(note.Id))
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

    // GET: Note/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Note == null)
        {
            return NotFound();
        }

        var note = await _context.Note
            .FirstOrDefaultAsync(m => m.Id == id);
        if (note == null)
        {
            return NotFound();
        }

        return View(note);
    }

    // POST: Note/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Note == null)
        {
            return Problem("Entity set 'NoteGoatContext.Note'  is null.");
        }
        var note = await _context.Note.FindAsync(id);
        if (note != null)
        {
            var user = await _context.User.FirstAsync(u => u.UserName == User.Identity.Name);
            var assignedRepos = await _context.Repo
                            .Include(r => r.Users)
                            .Where(r => r.Users.Contains(user))
                            .Select(r => r.Id)
                            .ToListAsync();

            if (!assignedRepos.Contains(note.RepoId))
            {
                return Forbid();
            }

            _context.Note.Remove(note);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool NoteExists(int id)
    {
        return (_context.Note?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
