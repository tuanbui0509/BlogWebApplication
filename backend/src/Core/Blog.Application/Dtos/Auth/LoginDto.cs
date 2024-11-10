using System.ComponentModel.DataAnnotations;

namespace Blog.Application.Common.Dtos.Auth
{
    public class LoginDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}