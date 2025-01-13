using Blog.Application.Abstractions;
using Blog.Application.Business.Authentication.Commands;
using Blog.Shared.Result;
using MediatR;

namespace Blog.Application.Business.Authentication.Handlers
{
    public class EnableTwoFactorCodeCommandHandler : IRequestHandler<EnableTwoFactorCodeCommand, IResult>
    {
        private readonly IAuthService _authService;

        public EnableTwoFactorCodeCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IResult> Handle(EnableTwoFactorCodeCommand request, CancellationToken cancellationToken)
        {
            return await _authService.VerifyTwoFactorCodeAsync(request.OTP, request.Email);
        }
    }
}