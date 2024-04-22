using Blog.Application.Common.Interfaces;
using Blog.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Blog.Persistence.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        
    }
}