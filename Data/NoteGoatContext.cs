using Microsoft.EntityFrameworkCore;
using NoteGoat.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace NoteGoat.Data;

public class NoteGoatContext : IdentityDbContext<User>
{
    public NoteGoatContext(DbContextOptions<NoteGoatContext> options)
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

    public DbSet<Attachment> Attachment { get; set; } = default!;
}
