using Blog.Application.Abstractions;
using Blog.Application.Business.Authentication.Commands;
using Blog.Application.Dtos.Auth;
using MediatR;

namespace Blog.Application.Business.Authentication.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResult>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _authService.LoginAsync(request.Email, request.Password);
        }
    }
}