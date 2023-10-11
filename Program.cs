using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NoteGoat.Data;
using Microsoft.AspNetCore.Identity;
using NoteGoat.Models;
using NoteGoat.Areas.Identity.Seed;
using NoteGoat.Logging;
using NoteGoat;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc();
builder.Services.AddDbContext<NoteGoatContext>(
        opts =>
                opts.UseSqlite(
                        builder.Configuration.GetConnectionString(
                                "NoteGoatContext"
                        )
                                ?? throw new InvalidOperationException(
                                        "Connection string 'NoteContext' not found."
                                )
                )
);

builder.Services
        .AddDefaultIdentity<User>(
                options => options.SignIn.RequireConfirmedAccount = true
        )
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<NoteGoatContext>();
builder.Services.ConfigureApplicationCookie(o =>
{
        o.SlidingExpiration = true;
        o.AccessDeniedPath = "/Identity/Account/AccessDenied";
});
builder.Logging.AddFile();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
        var services = scope.ServiceProvider;
        var roleManager = services.GetRequiredService<
                RoleManager<IdentityRole>
        >();
        await IdentityRoleSeeder.SeedRoles(roleManager);
}

app.Run();
