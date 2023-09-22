using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileGoat.Models;

public class Note
{
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        public required string Title { get; set; }

        [Required]
        public required string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; } = DateTime.Now;

        public virtual Attachment? Attachment { get; set; }

        [ForeignKey("Repo")]
        [DisplayName("Repository")]
        public int RepoId { get; set; }

        public virtual Repo? Repo { get; set; }

        public override string ToString()
        {
                return $"Id={Id},Title={Title}, Content={Content}, Created={Created}, Attachment={Attachment}, RepoId={RepoId}";
        }
}
