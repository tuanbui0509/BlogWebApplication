using Blog.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName, string description = "") : base(roleName)
        {
            Description = description;
        }

        public Roles Role { get; set; } // Using the enum for predefined roles
        public string Description { get; set; }
    }
}