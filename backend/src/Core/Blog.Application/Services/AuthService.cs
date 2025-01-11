using System.Text;
using Blog.Application.Abstractions;
using Blog.Application.Dtos.Auth;
using Blog.Application.Dtos.Email;
using Blog.Application.Interfaces;
using Blog.Domain.Enums;
using Blog.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Blog.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly Serilog.ILogger _logger;
        private readonly IUrlHelperService _urlHelperService;
        private readonly IEmailService _emailService;
        public AuthService(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IJwtTokenGenerator jwtTokenGenerator,
            IRefreshTokenRepository refreshTokenRepository,
            Serilog.ILogger logger,
            IEmailService emailService,
            IUrlHelperService urlHelperService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _logger = logger;
            _emailService = emailService;
            _urlHelperService = urlHelperService;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            email = email.ToUpperInvariant();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                _logger.Error("Invalid username or password", email);
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Invalid username or password" }
                };
            }
            if (!user.EmailConfirmed)
            {
                _logger.Error("Email was not confirmed by user", email);
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Please confirm your email before logging in." }
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            // Generate access and refresh tokens
            var accessToken = await _jwtTokenGenerator.GenerateAccessTokenAsync(user);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(user);
            // Create the result object to return
            return new AuthenticationResult
            {
                IsSuccess = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                UserId = user.Id.ToString(),
                Username = user.UserName,
                Roles = (List<string>)roles,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (storedToken == null || storedToken.IsUsed || storedToken.IsRevoked || storedToken.Expires <= DateTime.UtcNow)
            {
                return new AuthenticationResult { IsSuccess = false, Errors = { "Invalid refresh token" } };
            }

            // Mark as used and update
            storedToken.IsUsed = true;
            await _refreshTokenRepository.UpdateAsync(storedToken);

            var user = await _userManager.FindByIdAsync(storedToken.UserId);
            var newAccessToken = await _jwtTokenGenerator.GenerateAccessTokenAsync(user);
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken(user);

            await _refreshTokenRepository.AddAsync(newRefreshToken);

            return new AuthenticationResult
            {
                IsSuccess = true,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            };
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password, string username)
        {
            var user = new ApplicationUser { Email = email, UserName = username };
            user.NormalizedEmail = email.ToUpperInvariant();
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                _logger.Error(result.Errors.Select(e => e.Description).ToList().ToString(), email);
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            await _userManager.AddToRoleAsync(user, Roles.User.ToString());

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Generate callback URL
            var callbackUrl = _urlHelperService.GenerateCallbackUrl(
                "confirm-email",
                "api/user",
                new { userId = user.Id, token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token)) });

            // Send email
            await _emailService.SendEmailAsync(new EmailRequestDto()
            {
                To = email,
                Subject = "Code Study Mind Blog - Please activate your account",
                Body = $@"
    <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Account Activation</title>
            <style>
                body {{ font-family: Arial, sans-serif; background-color: #f7f7f7; margin: 0; padding: 0; }}
                .container {{ width: 100%; max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); }}
                .header {{ text-align: center; background-color: #4CAF50; padding: 10px; color: white; border-radius: 8px 8px 0 0; }}
                .header h1 {{ margin: 0; font-size: 24px; }}
                .body {{ padding: 20px; }}
                .body p {{ font-size: 16px; line-height: 1.6; color: #333333; }}
                .button {{ display: inline-block; background-color: #4CAF50; color: #ffffff; padding: 12px 20px; text-decoration: none; border-radius: 5px; font-size: 16px; text-align: center; }}
                .footer {{ text-align: center; margin-top: 30px; font-size: 12px; color: #888888; }}
                .footer p {{ margin: 0; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Account Activation</h1>
                </div>
                <div class='body'>
                    <p>Hello {email},</p>
                    <p>Thank you for registering with us! To complete your registration, please activate your account by clicking the link below:</p>
                    <p><a href='{callbackUrl}' class='button' style = 'color: #ffffff;'>Activate My Account</a></p>
                    <p>If you did not create an account with us, you can ignore this email.</p>
                </div>
                <div class='footer'>
                    <p>&copy; {DateTime.Now.Year} Your Company Name. All Rights Reserved.</p>
                    <p>If you have any questions, feel free to <a href='mailto:support@yourcompany.com'>contact us</a>.</p>
                </div>
            </div>
        </body>
    </html>"
            });
            return new AuthenticationResult
            {
                IsSuccess = true,
                AccessToken = token,
                UserId = user.Id.ToString(),
                Roles = new List<string> { Roles.User.ToString() },
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }
    }
}