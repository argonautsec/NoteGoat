using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FileGoat.Models;

public class User : IdentityUser
{
        [Required]
        [StringLength(50, MinimumLength = 3)]

        [Key]
        public int UserId { get; set; }

        public List<Repo> Repos { get; } = new();
}