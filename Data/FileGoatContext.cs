using Microsoft.EntityFrameworkCore;
using FileGoat.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FileGoat.Data;

public class FileGoatContext : IdentityDbContext<User>
{
    public FileGoatContext(DbContextOptions<FileGoatContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(e => e.Repos)
            .WithMany(e => e.Users);
    }


    public DbSet<Note> Note { get; set; } = default!;

    public DbSet<Repo> Repo { get; set; } = default!;

    public DbSet<User> User { get; set; } = default!;
}
