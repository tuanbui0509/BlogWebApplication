using Blog.Application.Dtos.Email;

namespace Blog.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequestDto request);
    }
}