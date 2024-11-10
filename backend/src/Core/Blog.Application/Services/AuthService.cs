using Blog.Application.Abstractions;
using Blog.Application.Dtos.Auth;
using Blog.Domain.Enums;
using Blog.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Blog.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public AuthService(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IJwtTokenGenerator jwtTokenGenerator,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            email = email.ToUpperInvariant();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Invalid username or password" }
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
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            await _userManager.AddToRoleAsync(user, Roles.User.ToString());

            var token = await _jwtTokenGenerator.GenerateAccessTokenAsync(user);
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