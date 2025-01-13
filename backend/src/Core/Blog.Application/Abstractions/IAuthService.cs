using Blog.Application.Business.Authentication.Commands;
using Blog.Application.Dtos.Auth;
using Blog.Shared.Result;

namespace Blog.Application.Abstractions
{
    public interface IAuthService
    {
        Task<AuthenticationResult> RegisterAsync(RegisterCommand request);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string refreshToken);
        Task<IResult> SendTwoFactorCodeAsync(string email);
        Task<IResult> VerifyTwoFactorCodeAsync(string code, string email);
    }
}