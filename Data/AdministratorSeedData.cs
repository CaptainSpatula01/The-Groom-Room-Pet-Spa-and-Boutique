using Microsoft.AspNetCore.Identity;

public class AdministratorSeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        string[] roleNames = { "Admin", "User" };
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Assign the Admin role to a specific user (e.g., company owner's email)
        var adminEmail = "owner@doggrooming.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            // If the admin user doesn't exist, create it
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail
            };
            await userManager.CreateAsync(adminUser, "SecurePassword123!");  // Ensure strong password
        }
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}
