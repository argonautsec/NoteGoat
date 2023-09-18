using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FileGoat.Data;
using Microsoft.AspNetCore.Identity;
using FileGoat.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<FileGoatContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("FileGoatContext") ?? throw new InvalidOperationException("Connection string 'NoteContext' not found.")));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<FileGoatContext>();
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

app.Run();
