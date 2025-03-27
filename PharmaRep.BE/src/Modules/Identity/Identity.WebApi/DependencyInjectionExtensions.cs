using Identity.Application;
using Identity.Infrastructure;
using Identity.Infrastructure.Database;
using Identity.WebApi.Endpoints;
using Microsoft.AspNetCore.Builder;
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
        
        if (!app.Environment.IsDevelopment()) return app;
        
        await PharmaRepIdentityDbContext.ApplyMigrationsAsync(scope.ServiceProvider);
        await PharmaRepIdentityDbContext.SeedAdminUserAsync(scope.ServiceProvider);

        return app;
    }
}