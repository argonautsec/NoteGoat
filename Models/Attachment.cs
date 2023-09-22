using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileGoat.Models;

public class Attachment
{
        [Key]
        public int Id { get; set; }

        [MinLength(1)]
        [Required]
        public string Name { get; set; } = "";
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "";

        [ForeignKey("Note")]
        public int NoteId { get; set; }

        internal async static Task<Attachment> FromFormFileAsync(IFormFile formFile)
        {
                using MemoryStream memoryStream = new();

                await formFile.CopyToAsync(memoryStream);
                return new Attachment()
                {
                        Content = memoryStream.ToArray(),
                        ContentType = formFile.ContentType,
                        Name = formFile.FileName
                };
        }

        public override string ToString()
        {
                return $"Name: {Name}, Content: {Content.Length}, ContentType: {ContentType}";
        }
}