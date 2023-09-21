using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FileGoat.Data;
using FileGoat.Models;
using Microsoft.AspNetCore.Authorization;
using FileGoat.Areas.Identity.Models;
using FileGoat.ViewModels.Repo;
using Microsoft.AspNetCore.Identity;

namespace FileGoat.Controllers;

[Authorize(Roles = Role.Host)]
public class RepoController : Controller
{
    private readonly FileGoatContext _context;
    private readonly ILogger<RepoController> _logger;
    private readonly UserManager<User> _userManager;

    public RepoController(
        FileGoatContext context,
        ILogger<RepoController> logger,
        UserManager<User> userManager)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
    }

    // GET: Repo
    public async Task<IActionResult> Index()
    {
        return _context.Repo != null ?
                    View(await _context.Repo.Include(r => r.Users).ToListAsync()) :
                    Problem("Entity set 'FileGoatContext.Repo'  is null.");
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

        var repo = await _context.Repo.Include(r => r.Users)
                                .Where(r => r.Id == id)
                                .FirstAsync();
        if (repo == null)
        {
            return NotFound();
        }


        return View(new RepoEdit
        {
            Repo = repo,
            Members = repo.Users,
            NonMembers = await _context.User.Where(u => !repo.Users.Contains(u)).ToListAsync(),
        });
    }

    // POST: Repo/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, RepoModification model)
    {
        Repo? repo = await _context.Repo.Include(r => r.Users).FirstAsync(r => r.Id == id);
        if (repo == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return await Edit(model.RepoId);
        }

        repo.Name = model.RepoName;
        repo.Description = model.RepoDescription;

        foreach (string userId in model.DeleteUserIds ?? Array.Empty<string>())
        {
            User? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                continue;
            }
            if (!repo.Users.Remove(user))
            {
                _logger.LogWarning("couldn't remove user {} from repo", user.UserName);
            }

        }
        foreach (string userId in model.AddUserIds ?? Array.Empty<string>())
        {
            User? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                continue;
            }
            repo.Users.Add(user);
        }

        await _context.SaveChangesAsync();
        return await Edit(id);
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
