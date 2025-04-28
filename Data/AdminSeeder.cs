using Library.Models;
using Microsoft.AspNetCore.Identity;

namespace Library.Data
{
    public class AdminSeeder
    {

        public static async Task CreateAdminAccount(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string adminUserName = "admin123";
            string adminEmail = "admin@example.com";
            string adminPassword = "9kLuFn3j";

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminUserName,
                    Email = adminEmail
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
