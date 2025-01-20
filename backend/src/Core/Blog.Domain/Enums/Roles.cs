using System.ComponentModel;

namespace Blog.Domain.Enums
{
    public enum Roles
    {
        [Description("SuperAdmin")]
        SuperAdmin,
        
        [Description("Admin")]
        Admin,
        
        [Description("User")]
        User
    }
}