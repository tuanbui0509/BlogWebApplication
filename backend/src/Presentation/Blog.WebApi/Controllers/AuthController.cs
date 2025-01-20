using System.Security.Claims;
using Blog.Application.Abstractions;
using Blog.Domain.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Blog.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IAuthService authService, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authService = authService;
            _configuration = configuration;
        }

        [HttpGet("login/{provider}")]
        public IActionResult Login(string provider)
        {
            if (string.IsNullOrEmpty(provider)) return BadRequest("Provider is required.");

            var supportedProviders = new[] { "Google", "Facebook", "GitHub" };
            if (!supportedProviders.Contains(provider, StringComparer.OrdinalIgnoreCase))
                return BadRequest("Unsupported provider.");

            var redirectUrl = Url.Action(
                action: "Callback",
                controller: "Auth",
                values: new { provider },
                protocol: Request.Scheme
            );

            var properties = new AuthenticationProperties { RedirectUri = redirectUrl ,Items = { { "state", Guid.NewGuid().ToString() }}}; // Store the state};
            return Challenge(properties, provider);
        }

        [HttpGet("callback/{provider}")]
        public async Task<IActionResult> Callback(string provider)
        {
            Console.WriteLine($"Received callback for provider: {provider}");

            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded) return BadRequest("External authentication error.");

            var claims = authenticateResult.Principal?.Claims;
            var userId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            // Generate JWT Token
            var token = GenerateJwtToken(name, email, userId);

            return Ok(new { token, user = new { name, email, userId } });
        }

        private string GenerateJwtToken(string name, string email, string userId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JwtSettings:SigningKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
