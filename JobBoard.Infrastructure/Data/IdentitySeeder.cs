using JobBoard.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoard.Infrastructure.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles =
        {
            "Admin",
            "Employer",
            "Seeker"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(role));

                if (!roleResult.Succeeded)
                {
                    throw new Exception(
                        $"Failed to create role '{role}'. " +
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }
        }

        const string adminEmail = "admin@jobboard.com";
        const string adminPassword = "Admin@123";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var userResult = await userManager.CreateAsync(adminUser, adminPassword);

            if (!userResult.Succeeded)
            {
                throw new Exception(
                    $"Failed to create admin user. " +
                    string.Join(", ", userResult.Errors.Select(e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            var addRoleResult = await userManager.AddToRoleAsync(adminUser, "Admin");

            if (!addRoleResult.Succeeded)
            {
                throw new Exception(
                    $"Failed to add admin user to Admin role. " +
                    string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
            }
        }
    }
}