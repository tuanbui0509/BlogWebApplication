using System.Net;
using System.Net.Mail;
using Blog.Application.Dtos.Email;
using Blog.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Blog.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly Serilog.ILogger _logger;

        public EmailService(IConfiguration configuration, Serilog.ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(EmailRequestDto request)
        {
            try
            {
                using var smtp = new SmtpClient
                {
                    Host = _configuration["EmailConfiguration:Host"],
                    Port = int.Parse(_configuration["EmailConfiguration:Port"]),
                    Credentials = new NetworkCredential(
                        _configuration["EmailConfiguration:Username"],
                        _configuration["EmailConfiguration:Password"]
                    ),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["EmailConfiguration:From"]),
                    Subject = request.Subject,
                    Body = request.Body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(request.To);
                await smtp.SendMailAsync(mailMessage);
                _logger.Information($"Email sent to {request.To} with subject: {request.Subject}");
            }
            catch (SmtpException ex)
            {
                _logger.Error($"SMTP Exception: {ex.Message}");
                if (ex.InnerException != null)
                {
                    _logger.Error($"Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected error: {ex.Message}");
                throw;
            }

        }
    }
}