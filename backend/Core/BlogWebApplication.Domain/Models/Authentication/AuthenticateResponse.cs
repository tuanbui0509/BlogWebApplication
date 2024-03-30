using BlogWebApplication.Shared.Implements;

namespace BlogWebApplication.Domain.Models.Authentication
{
    public class AuthenticateResponse : Result<bool>
    {
        public string? Email { get; set; }

        public string Token { get; set; }
    }
}