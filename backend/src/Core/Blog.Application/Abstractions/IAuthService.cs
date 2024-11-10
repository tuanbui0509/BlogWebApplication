using Blog.Application.Dtos.Auth;

namespace Blog.Application.Abstractions
{
    public interface IAuthService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password, string username);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string refreshToken);
    }
}