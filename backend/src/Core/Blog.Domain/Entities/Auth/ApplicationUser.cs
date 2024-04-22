using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // Add additional properties as needed for your blog users
        public string FullName { get; set; }
        public string Email { get; set; }

        // Navigation properties
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}