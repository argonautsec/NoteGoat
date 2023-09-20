using System.ComponentModel.DataAnnotations;

namespace FileGoat.ViewModels.Repo;

public class RepoModification
{
        public int RepoId { get; set; }
        [Required]
        public string RepoName { get; set; } = "";
        public string RepoDescription { get; set; } = "";
        public string[]? AddUserIds { get; set; }
        public string[]? DeleteUserIds { get; set; }
}