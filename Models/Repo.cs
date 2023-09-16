using System.ComponentModel.DataAnnotations;

namespace FileGoat.Models;

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