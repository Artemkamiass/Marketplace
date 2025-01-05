using Marketplace.Models;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Services
{
    public class RoleInitializer
    {
        public static async Task Initialize(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminEmail = "admin@gmail.ru";
            var password = "Password1!";
            if(await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if(await userManager.FindByNameAsync(adminEmail) == null)
            {
                var user = new ApplicationUser { UserName = adminEmail, Email = adminEmail};

                var result = await userManager.CreateAsync(user, password);
                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
