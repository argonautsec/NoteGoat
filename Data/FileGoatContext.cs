using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FileGoat.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FileGoat.Data
{
    public class FileGoatContext : IdentityDbContext<User>
    {
        public FileGoatContext(DbContextOptions<FileGoatContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Repos)
                .WithMany(e => e.Users);
        }


        public DbSet<FileGoat.Models.Note> Note { get; set; } = default!;
    }
}
