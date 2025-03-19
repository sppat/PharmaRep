using System.Security.Claims;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.Database;

public class PharmaRepIdentityDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public PharmaRepIdentityDbContext(DbContextOptions<PharmaRepIdentityDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(typeof(PharmaRepIdentityDbContext).Assembly);
    }
    
    public static async Task ApplyMigrationsAsync(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<PharmaRepIdentityDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
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