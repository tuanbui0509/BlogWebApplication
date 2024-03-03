using Microsoft.AspNetCore.Identity;

namespace BlogWebApplication.Domain.Entities.Authentication
{
    public class UserApplication : IdentityUser
    {
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}