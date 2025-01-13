using Blog.Shared.Result;
using MediatR;

namespace Blog.Application.Business.Authentication.Commands
{
    public class TwoFactorCodeCommand : IRequest<IResult>
    {
        public string Email { get; set; }
    }
}