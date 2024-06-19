using Blog.Application.Common.Dtos.Email;

namespace Blog.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequestDto request);
    }
}