using BlogWebApplication.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace BlogWebApplication.Domain.Entities.Authentication
{
    public partial class UserApplication : IdentityUser
    {
        public Roles Role { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}