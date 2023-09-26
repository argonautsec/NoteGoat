using NoteGoat.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace NoteGoat.Areas.Identity.Seed
{
        public class IdentityRoleSeeder
        {

                public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
                {
                        await CreateRole(roleManager, Role.Admin);
                        await CreateRole(roleManager, Role.Host);
                        await CreateRole(roleManager, Role.User);
                }

                public static async Task CreateRole(RoleManager<IdentityRole> roleManager, string name)
                {
                        var role = await roleManager.FindByNameAsync(name);
                        if (role == null)
                        {
                                var res = await roleManager.CreateAsync(new IdentityRole(name));
                                if (!res.Succeeded)
                                {
                                        throw new Exception("Failed to create role " + name);
                                }
                        }
                }
        }
}