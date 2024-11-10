using Blog.Application.Dtos.Auth;
using MediatR;

namespace Blog.Application.Business.Authentication.Commands
{
    public class LoginCommand : IRequest<AuthenticationResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}