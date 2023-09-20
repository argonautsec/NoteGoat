using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileGoat.Models;

public class Note
{
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; } = DateTime.Now;

        public string? FileName { get; set; }

        public byte[]? FileContent { get; set; }

        [ForeignKey("Repo")]
        public int RepoId { get; set; }
}
