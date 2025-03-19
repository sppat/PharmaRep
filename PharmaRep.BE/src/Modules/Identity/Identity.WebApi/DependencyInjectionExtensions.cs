using Identity.Infrastructure;
using Identity.Infrastructure.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.WebApi;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddIdentityWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityInfrastructure(configuration);
        return services;
    }
    
    public static async  Task<IApplicationBuilder> UseIdentityMiddleware(this IApplicationBuilder app, bool isDevelopmentEnvironment)
    {
        using var scope = app.ApplicationServices.CreateScope();
        await PharmaRepIdentityDbContext.ApplyMigrationsAsync(scope.ServiceProvider);
        
        if (isDevelopmentEnvironment) await PharmaRepIdentityDbContext.SeedAdminUserAsync(scope.ServiceProvider);
        return app;
    }
}