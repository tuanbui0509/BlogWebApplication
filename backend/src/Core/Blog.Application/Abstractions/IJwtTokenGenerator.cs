using Blog.Domain.Entities;
using Blog.Domain.Identity;

namespace Blog.Application.Abstractions
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateAccessTokenAsync(ApplicationUser user);
        RefreshToken GenerateRefreshToken(ApplicationUser user);
    }
}