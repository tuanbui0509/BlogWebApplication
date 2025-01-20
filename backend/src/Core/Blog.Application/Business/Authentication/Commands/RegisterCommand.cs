using Blog.Application.Dtos.Auth;
using MediatR;

namespace Blog.Application.Business.Authentication.Commands
{
    public class RegisterCommand: IRequest<AuthenticationResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}