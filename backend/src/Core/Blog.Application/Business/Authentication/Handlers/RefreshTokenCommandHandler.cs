using Blog.Application.Abstractions;
using Blog.Application.Business.Authentication.Commands;
using Blog.Application.Dtos.Auth;
using MediatR;

namespace Blog.Application.Business.Authentication.Handlers
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthenticationResult>
    {
        private readonly IAuthService _authService;

        public RefreshTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthenticationResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RefreshTokenAsync(request.RefreshToken);
        }
    }
}