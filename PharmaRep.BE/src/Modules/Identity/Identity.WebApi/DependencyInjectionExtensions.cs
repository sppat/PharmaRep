using Identity.Application;
using Identity.Domain.Entities;
using Identity.Infrastructure;
using Identity.Infrastructure.Database;
using Identity.WebApi.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity.WebApi;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddIdentityWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityInfrastructure(configuration)
            .AddIdentityApplication();
        
        return services;
    }
    
    public static async  Task<WebApplication> UseIdentityMiddleware(this WebApplication app, IServiceScope scope)
    {
        app.MapUserEndpoints();
        
        if (app.Environment.IsProduction()) return app;

        await using var identityDbContext = scope.ServiceProvider.GetRequiredService<PharmaRepIdentityDbContext>();
        await identityDbContext.Database.MigrateAsync();

        using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        await IdentitySeeder.SeedAdminUserAsync(configuration, userManager);

        return app;
    }
}