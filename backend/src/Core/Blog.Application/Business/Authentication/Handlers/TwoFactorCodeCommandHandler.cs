using Blog.Application.Abstractions;
using Blog.Application.Business.Authentication.Commands;
using Blog.Shared.Result;
using MediatR;

namespace Blog.Application.Business.Authentication.Handlers
{
    public class TwoFactorCodeCommandHandler : IRequestHandler<TwoFactorCodeCommand, IResult>
    {
        private readonly IAuthService _authService;

        public TwoFactorCodeCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IResult> Handle(TwoFactorCodeCommand request, CancellationToken cancellationToken)
        {
            return await _authService.SendTwoFactorCodeAsync(request.Email);
        }
    }
}