using Blog.Domain.Enums;
using Blog.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Blog.Persistence.Data.Seeds
{
    public class SeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public SeedData(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Initialize(IServiceProvider serviceProvider)
        {
            // await SetUpRoles();
            // await SeedUsersAsync();
        }

        public async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(Roles)).Cast<Roles>())
            {
                var roleName = role.ToString();
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = roleName, Description = roleName });
                }
            }
        }

        public async Task SetUpRoles()
        {

            await SeedRolesAsync(_roleManager);
        }

        public async Task SeedUsersAsync()
        {
            await SeedSuperAdminAsync();
            await SeedAdminAsync();
            await SeedBasicUserAsync();
        }
        public async Task SeedSuperAdminAsync()
        {
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin@gmail.com",
                Email = "superadmin@gmail.com",
                FullName = "Super Admin User",
                EmailConfirmed = true,
                NormalizedEmail = "superadmin@gmail.com",
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
                NormalizedEmail = "admin@gmail.com",
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
                NormalizedEmail = "basicuser@gmail.com",
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