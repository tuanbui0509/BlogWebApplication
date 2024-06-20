using Blog.Domain.Identity;

namespace Blog.Application.Common.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(ApplicationUser user);
    }
}