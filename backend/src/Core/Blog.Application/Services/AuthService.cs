using System.Text;
using Blog.Application.Abstractions;
using Blog.Application.Business.Authentication.Commands;
using Blog.Application.Dtos.Auth;
using Blog.Application.Dtos.Email;
using Blog.Application.Interfaces;
using Blog.Domain.Enums;
using Blog.Domain.Identity;
using Blog.Shared.Result;
using Microsoft.AspNetCore.Http;
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
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IJwtTokenGenerator jwtTokenGenerator,
            IRefreshTokenRepository refreshTokenRepository,
            Serilog.ILogger logger,
            IEmailService emailService,
            IUrlHelperService urlHelperService,
            SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _logger = logger;
            _emailService = emailService;
            _urlHelperService = urlHelperService;
            _signInManager = signInManager;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email.ToUpperInvariant());
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

        public async Task<AuthenticationResult> RegisterAsync(RegisterCommand request)
        {
            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.UserName,
                NormalizedUserName = request.UserName.Normalize(),
                FullName = request.FullName,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            
            user.NormalizedEmail = user.Email.ToUpperInvariant(); // Normalized
            await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                _logger.Error(result.Errors.Select(e => e.Description).ToList().ToString(), request.Email);
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
                To = request.Email,
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
                    <p>Hello {request.FullName},</p>
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

        public async Task<Shared.Result.IResult> SendTwoFactorCodeAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email.Normalize());
            if (user == null)
            {
                return new FailureResult("Unable to load user.", StatusCodes.Status404NotFound);
            }

            // Generate a 2FA token for email-based authentication
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();
            // Store the token in the user manager (as a claim or custom store)
            var result = await _userManager.SetAuthenticationTokenAsync(user, "Email", "TwoFactorCode", code);
            if (!result.Succeeded) return new FailureResult("Failed to save 2FA code.", StatusCodes.Status404NotFound);

            // Send the 2FA code to the user's email
            var emailSubject = "Code Study Mind - Your 2FA Code";
            var emailBody = File.ReadAllText("templates/Email/EmailOTPTemplate.html");
            emailBody = emailBody.Replace("{{OTP_CODE}}", code);
            await _emailService.SendEmailAsync(new EmailRequestDto
            {
                To = email,
                Subject = emailSubject,
                Body = emailBody
            });

            // Redirect to the page where the user can input the 2FA code
            return new SuccessResult("Sent email", StatusCodes.Status200OK);
        }

        public async Task<Shared.Result.IResult> VerifyTwoFactorCodeAsync(string code, string email)
        {
            var user = await _userManager.FindByEmailAsync(email.Normalize());

            if (user == null)
            {
                return new FailureResult("Unable to load user.", StatusCodes.Status404NotFound);
            }

            // Validate the 2FA code entered by the user
            var storedToken = await _userManager.GetAuthenticationTokenAsync(user, "Email", "TwoFactorCode");
            if (storedToken == null || storedToken != code)
            {
                return new FailureResult("Invalid or expired code.");
            }

            // Remove the token after successful verification
            await _userManager.RemoveAuthenticationTokenAsync(user, "Email", "TwoFactorCode");

            // Enable TwoFactorEnabled for the user
            user.TwoFactorEnabled = true; // Update the field
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return new FailureResult("Failed to enable two-factor authentication.");
            }

            return new SuccessResult("2FA verified and enabled successfully.");
        }
    }
}