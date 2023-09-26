using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NoteGoat.Data;
using Microsoft.AspNetCore.Identity;
using NoteGoat.Models;
using NoteGoat.Areas.Identity.Seed;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc();
builder.Services.AddDbContext<NoteGoatContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("NoteGoatContext") ?? throw new InvalidOperationException("Connection string 'NoteContext' not found.")));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<NoteGoatContext>();
builder.Services.ConfigureApplicationCookie(o =>
{
    o.SlidingExpiration = true;
    o.AccessDeniedPath = "/Identity/Account/AccessDenied";
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentityRoleSeeder.SeedRoles(roleManager);
}

app.Run();
