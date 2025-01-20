using System.Security.Claims;
using System.Text;
using Blog.Application.Business.Authentication.Commands;
using Blog.Application.Common.Dtos.Auth;
using Blog.Domain.Constants;
using Blog.Domain.Enums;
using Blog.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Blog.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ApiControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly Serilog.ILogger _logger;
        private readonly IMediator _mediator;

        public UserController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IMediator mediator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = Log.ForContext<PostsController>();
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
                return Ok(result);
            return Unauthorized(result.Errors);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand request)
        {
            var result = await _mediator.Send(new RefreshTokenCommand { RefreshToken = request.RefreshToken });

            if (!result.IsSuccess)
                return Unauthorized(result.Errors);

            return Ok(new { result.AccessToken, result.RefreshToken });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
                return Ok("Registration successful. Please check your email to confirm your account.");

            return BadRequest(result.Errors);
        }

        [Authorize(Roles = nameof(Roles.SuperAdmin))]
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return BadRequest(new { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                FullName = model.FullName,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(
                    new { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new ApplicationRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new ApplicationRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok(new { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                //create the roles and seed them to the database: Question 1
                var roleResult = await _roleManager.CreateAsync(new ApplicationRole(roleName));

                if (roleResult.Succeeded)
                {
                    _logger.Information("Roles Added");
                    return Ok(new { result = $"Role {roleName} added successfully" });
                }
                else
                {
                    _logger.Information("Error");
                    return BadRequest(new { error = $"Issue adding the new {roleName} role" });
                }
            }

            return BadRequest(new { error = "Role already exist" });
        }

        // Add User to role
        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email.ToLower());

            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    _logger.Information("User {Email} added to the {Role} role", user.Email, roleName);
                    return Ok(new { result = $"User {user.Email} added to the {roleName} role" });
                }
                else
                {
                    _logger.Information("Error: Unable to add user {Email} to the {Role} role", user.Email, roleName);
                    return BadRequest(new { error = $"Error: Unable to add user {user.Email} to the {roleName} role" });
                }
            }

            // User doesn't exist
            return BadRequest(new { error = "Unable to find user" });
        }

        // Remove User to role
        [HttpPost]
        [Route("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    _logger.Information($"User {user.Email} removed from the {roleName} role");
                    return Ok(new { result = $"User {user.Email} removed from the {roleName} role" });
                }
                else
                {
                    _logger.Information($"Error: Unable to removed user {user.Email} from the {roleName} role");
                    return BadRequest(new { error = $"Error: Unable to removed user {user.Email} from the {roleName} role" });
                }
            }

            // User doesn't exist
            return BadRequest(new { error = "Unable to find user" });
        }

        [Authorize(Roles = $"{nameof(Roles.SuperAdmin)},{nameof(Roles.Admin)}")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        // 2. Custom Policy-based Authorization
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        // 3. Checking roles programmatically within an action
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("GetUserRoles")]
        public IActionResult GetUserRoles()
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role) // Or use c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" if using custom claim
                .Select(c => c.Value)
                .ToList();

            if (roles.Contains("SuperAdmin"))
            {
                // Logic specific to SuperAdmin users
                return Ok("User is a SuperAdmin");
            }
            else if (roles.Contains("Admin"))
            {
                // Logic specific to Admin users
                return Ok("User is an Admin");
            }
            else
            {
                return Ok("User has no specific role permissions");
            }
        }

        // 4. Programmatic multi-role access within action
        [Authorize]
        [HttpGet("AdminOrUserAccess")]
        public IActionResult AdminOrUserAccess()
        {
            var userRoles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (userRoles.Contains("Admin") || userRoles.Contains("User"))
            {
                return Ok("Access granted for Admin or User.");
            }

            return Unauthorized("Access denied.");
        }

        [AllowAnonymous]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return BadRequest("Invalid email confirmation request.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            // Decode the token
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                return BadRequest("Email confirmation failed.");

            return Ok("Email confirmed successfully.");
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyTwoFactorCode(EnableTwoFactorCodeCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpPost("disable-2fa")]
        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(Authenticate));
            }

            // Disable 2FA for the user
            var result = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (result.Succeeded)
            {
                return Ok("Disable successful");
            }

            return BadRequest("An error occurred while disabling 2FA.");
        }

        [HttpPost("enable-2fa")]
        public async Task<IActionResult> EnableTwoFactorAuthentication(TwoFactorCodeCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.Message);
        }

    }
}