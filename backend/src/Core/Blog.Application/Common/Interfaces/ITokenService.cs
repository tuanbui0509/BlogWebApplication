using Blog.Domain.Identity;

namespace Blog.Application.Common.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user);
    }
}