using groomroom.Entities;
using Microsoft.AspNetCore.Identity;

public class AdministratorSeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        string[] roleNames = {"Admin", "User"};
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var role = new Role { Name = roleName };
                await roleManager.CreateAsync(role);
            }
        }

        var adminEmail = "owner@doggrooming.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "User"
            };
            await userManager.CreateAsync(adminUser, "SecurePassword123!");  // Ensure strong password
        }
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}