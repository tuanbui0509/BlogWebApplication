using Blog.Application.Dtos.Auth;
using MediatR;

namespace Blog.Application.Business.Authentication.Commands
{
    public class RefreshTokenCommand: IRequest<AuthenticationResult>
    {
        public string RefreshToken { get; set; }
    }
}