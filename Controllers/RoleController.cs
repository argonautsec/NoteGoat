using System.ComponentModel.DataAnnotations;
using NoteGoat.Areas.Identity.Models;
using NoteGoat.Models;
using NoteGoat.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace NoteGoat.Controllers;

[Authorize(Roles = Role.Admin)]
public class RoleController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;

    public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public IActionResult Index() => View(_roleManager.Roles);

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create([Required] string name)
    {
        if (ModelState.IsValid)
        {
            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
            if (result.Succeeded)
                return RedirectToAction("Index");
            else
                Errors(result);
        }
        return View(name);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        IdentityRole? role = await _roleManager.FindByIdAsync(id);
        if (role != null)
        {
            IdentityResult result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return RedirectToAction("Index");
            else
                Errors(result);
        }
        else
            ModelState.AddModelError("", "No role found");
        return View("Index", _roleManager.Roles);
    }

    private void Errors(IdentityResult result)
    {
        foreach (IdentityError error in result.Errors)
            ModelState.AddModelError("", error.Description);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(RoleModification model)
    {
        IdentityResult result;
        if (ModelState.IsValid)
        {
            foreach (string userId in model.AddIds ?? new string[] { })
            {
                User? user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    result = await _userManager.AddToRoleAsync(user, model.RoleName);
                    if (!result.Succeeded)
                        Errors(result);
                }
            }
            foreach (string userId in model.DeleteIds ?? new string[] { })
            {
                User? user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                    if (!result.Succeeded)
                        Errors(result);
                }
            }
        }

        if (ModelState.IsValid)
            return RedirectToAction(nameof(Index));
        else
            return await Edit(model.RoleId);

    }

    public async Task<IActionResult> Edit(string id)
    {
        IdentityRole? role = await _roleManager.FindByIdAsync(id);
        List<User> members = new List<User>();
        List<User> nonMembers = new List<User>();
        foreach (User user in _userManager.Users)
        {
            var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
            list.Add(user);
        }
        return View(new RoleEdit
        {
            Role = role,
            Members = members,
            NonMembers = nonMembers
        });
    }

}