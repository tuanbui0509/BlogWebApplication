using System.ComponentModel.DataAnnotations;

namespace BlogWebApplication.Domain.Models.Authentication
{
    public class AuthenticateRequest
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}