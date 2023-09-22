using System.ComponentModel.DataAnnotations;

namespace FileGoat.ViewModels.Repo;

public class RepoModification
{
        public int RepoId { get; set; }
        [Required]
        public string RepoName { get; set; } = "";
        public string RepoDescription { get; set; } = "";
        public string[] AddUserIds { get; set; } = Array.Empty<string>();
        public string[] DeleteUserIds { get; set; } = Array.Empty<string>();

        public override string ToString()
        {
                return $"RepoId={RepoId}, RepoName={RepoName}, RepoDescription={RepoDescription}, AddUserIds={string.Join(", ", AddUserIds)}, DeleteUserIds={string.Join(", ", DeleteUserIds)}";
        }
}