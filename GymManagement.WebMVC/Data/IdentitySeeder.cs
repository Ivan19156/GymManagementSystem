using Microsoft.AspNetCore.Identity;

namespace GymManagement.WebMVC.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        UserManager<IdentityUser>  userManager,
        RoleManager<IdentityRole>  roleManager)
    {
        foreach (var role in new[] { "Admin", "Manager" })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        const string adminEmail    = "admin@gym.com";
        const string adminPassword = "Admin123!";

        if (await userManager.FindByEmailAsync(adminEmail) is null)
        {
            var admin = new IdentityUser
            {
                UserName       = adminEmail,
                Email          = adminEmail,
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}
