using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FileGoat.Data;
using FileGoat.Models;
using Microsoft.AspNetCore.Authorization;
using FileGoat.Areas.Identity.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FileGoat;

[Authorize(Roles = Role.User)]
public class NoteController : Controller
{
    private readonly FileGoatContext _context;
    private readonly ILogger<NoteController> _logger;

    public NoteController(FileGoatContext context, ILogger<NoteController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Note
    public async Task<IActionResult> Index()
    {
        return _context.Note != null ?
                    View(await _context.Note.ToListAsync()) :
                    Problem("Entity set 'FileGoatContext.Note'  is null.");
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

    // POST: Note/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Attachment")] IFormFile? attachment,
        [Bind("Id,Title,Content,Created,RepoId")] Note note)
    {
        if (!ModelState.IsValid)
        {
            return View(note);
        }

        if (attachment != null)
        {

            using MemoryStream memoryStream = new();

            await attachment.CopyToAsync(memoryStream);
            note.FileName = attachment.FileName;
            note.FileContent = memoryStream.ToArray();
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
        [Bind("Attachment")] IFormFile? attachment,
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

        if (attachment != null)
        {
            _logger.LogInformation("Attachment file name {}", attachment.FileName);

            using MemoryStream memoryStream = new();

            await attachment.CopyToAsync(memoryStream);
            note.FileName = attachment.FileName;
            note.FileContent = memoryStream.ToArray();
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
            return Problem("Entity set 'FileGoatContext.Note'  is null.");
        }
        var note = await _context.Note.FindAsync(id);
        if (note != null)
        {
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
