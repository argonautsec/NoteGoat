using NoteGoat.Models;

namespace NoteGoat.ViewModels.Repo;

public class RepoEdit
{

        public Models.Repo? Repo { get; set; }
        public IEnumerable<User> Members { get; set; } = new List<User>();
        public IEnumerable<User> NonMembers { get; set; } = new List<User>();
}