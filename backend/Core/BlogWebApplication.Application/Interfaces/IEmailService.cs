using BlogWebApplication.Application.Dtos.Emails;

namespace BlogWebApplication.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequestDto request);
    }
}