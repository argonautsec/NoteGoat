using FileGoat.Models;
using Microsoft.AspNetCore.Identity;

namespace FileGoat.ViewModels.Role
{
        public class RoleEdit
        {
                public IdentityRole? Role { get; set; }
                public IEnumerable<User> Members { get; set; } = new List<User>();
                public IEnumerable<User> NonMembers { get; set; } = new List<User>();
        }
}