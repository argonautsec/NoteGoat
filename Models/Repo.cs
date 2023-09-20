using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FileGoat.Models;

[Index(nameof(Name), IsUnique = true)]
public class Repo
{

        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }

        public List<Note> Notes { get; } = new();
        public List<User> Users { get; } = new();
}