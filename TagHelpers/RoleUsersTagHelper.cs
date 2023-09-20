using FileGoat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FileGoat.TagHelpers;

[HtmlTargetElement("td", Attributes = "i-role")]
public class RoleUsersTagHelper : TagHelper
{
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleUsersTagHelper(UserManager<User> usermgr, RoleManager<IdentityRole> rolemgr)
        {
                _userManager = usermgr;
                _roleManager = rolemgr;
        }

        [HtmlAttributeName("i-role")]
        public string Role { get; set; } = "";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
                List<string> names = new List<string>();
                IdentityRole? role = await _roleManager.FindByIdAsync(Role);
                if (role != null)
                {
                        foreach (var user in _userManager.Users)
                        {
                                if (user != null && await _userManager.IsInRoleAsync(user, role.Name))
                                        names.Add(user.UserName);
                        }
                }
                output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names));
        }
}