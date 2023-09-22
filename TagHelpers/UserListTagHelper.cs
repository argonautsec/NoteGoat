using FileGoat.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FileGoat.TagHelpers;

[HtmlTargetElement("span", Attributes = "i-users")]
public class UserListTagHelper : TagHelper
{

        [HtmlAttributeName("i-users")]
        public required IEnumerable<User> Users { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
                List<string> names = new();
                {
                        foreach (var user in Users)
                        {
                                names.Add(user.UserName);
                        }
                }
                output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names));
                return Task.CompletedTask;
        }
}