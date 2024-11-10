using Blog.Application.Abstractions;
using Blog.Application.Business.Authentication.Commands;
using Blog.Application.Dtos.Auth;
using MediatR;

namespace Blog.Application.Business.Authentication.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResult>
    {
        private readonly IAuthService _authService;

        public RegisterCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthenticationResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RegisterAsync(request.Email, request.Password, request.UserName);
        }
    }
}