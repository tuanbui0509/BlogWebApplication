using BlogWebApplication.Domain.Entities.Authentication;
using BlogWebApplication.Domain.Enums;
using BlogWebApplication.Domain.Models.Authentication;
using BlogWebApplication.Domain.Models.Register;
using BlogWebApplication.Infrastructure.Services.Tokens;
using BlogWebApplication.Persistence.Contexts;
using BlogWebApplication.Shared.Implements;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebApplication.WebApi.Controllers
{
    public class UsersController : ApiControllerBase
    {
        private readonly UserManager<UserApplication> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;

        public UsersController(
            UserManager<UserApplication> userManager,
            ApplicationDbContext context,
            TokenService tokenService)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userManager.CreateAsync(
                new UserApplication { UserName = request.Username, Email = request.Email, Role = Roles.User },
                request.Password!
            );

            if (result.Succeeded)
            {
                request.Password = "";
                return CreatedAtAction(nameof(Register), new { email = request.Email, role = request.Role }, request);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }


        [HttpPost]
        [Route("login")]
        public async Task<Result<AuthenticateResponse>> Authenticate([FromBody] AuthenticateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return await Result<AuthenticateResponse>.FailureAsync("Login Fails.");
            }

            var managedUser = await _userManager.FindByEmailAsync(request.Email!);
            if (managedUser == null)
            {
                return await Result<AuthenticateResponse>.FailureAsync("Login Fails.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password!);
            if (!isPasswordValid)
            {
                return await Result<AuthenticateResponse>.FailureAsync("Login Fails.");
            }

            var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if (userInDb is null)
            {
                return null;
            }

            var accessToken = _tokenService.CreateToken(userInDb);
            await _context.SaveChangesAsync();
            return await Result<AuthenticateResponse>.SuccessAsync(new AuthenticateResponse
            {
                Email = userInDb.Email,
                Token = accessToken,
            }, "Login Fails.");
        }
    }
}