using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Identity.Infrastructure.Database;

public class IdentitySeeder(IConfiguration configuration, UserManager<User> userManager)
{
    public async Task SeedAdminUserAsync()
    {
        var adminEmail = configuration["AdminCredentials:Email"];
        var adminPassword = configuration["AdminCredentials:Password"];
        
        var admin = await userManager.FindByEmailAsync(adminEmail!);

        if (admin is not null) return;
        
        admin = User.Create(email: configuration["AdminCredentials:Email"], 
            firstName: "admin", 
            lastName: "admin");
        
        await userManager.CreateAsync(admin, adminPassword!);
        await userManager.AddToRoleAsync(admin, Role.Admin.Name!);
    }
}