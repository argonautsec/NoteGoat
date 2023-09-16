using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FileGoat.Models;

public class User : IdentityUser
{
        [Required]
        [StringLength(50, MinimumLength = 3)]

        public List<Repo> Repos { get; } = new();
}