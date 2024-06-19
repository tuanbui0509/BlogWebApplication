using Blog.Domain.Enums;
using Blog.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Blog.Persistence.Data.Seeds
{
    public class SeedData
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedData(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task SetUpRoles()
        {
            var roles = _config.GetSection("Roles").Get<IEnumerable<string>>();
            // await _roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            // await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            // await _roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
        }

        public async Task SeedSuperAdminAsync()
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin@gmail.com",
                Email = "superadmin@gmail.com",
                FullName = "Super Admin User",
                EmailConfirmed = true
            };
            if (_userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await _userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await _userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await _userManager.AddToRoleAsync(defaultUser, Roles.User.ToString());
                    await _userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await _userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                }
            }
        }

        public async Task SeedAdminAsync()
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                FullName = "Admin User",
                EmailConfirmed = true
            };
            if (_userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await _userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await _userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await _userManager.AddToRoleAsync(defaultUser, Roles.User.ToString());
                    await _userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                }
            }
        }

        public async Task SeedBasicUserAsync()
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "basicuser@gmail.com",
                Email = "basicuser@gmail.com",
                EmailConfirmed = true,
                FullName = "Basic User"
            };
            if (_userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await _userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await _userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await _userManager.AddToRoleAsync(defaultUser, Roles.User.ToString());
                }
            }
        }
    }
}