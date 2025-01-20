using Blog.Shared.Result;
using MediatR;

namespace Blog.Application.Business.Authentication.Commands
{
    public class EnableTwoFactorCodeCommand : IRequest<IResult>
    {
        public string OTP { get; set; }
        public string Email { get; set; }
    }
}