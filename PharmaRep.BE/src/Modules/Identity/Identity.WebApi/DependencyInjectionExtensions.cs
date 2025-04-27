using Identity.Application;
using Identity.Infrastructure;
using Identity.Infrastructure.Database;
using Identity.WebApi.Endpoints;
using Microsoft.AspNetCore.Builder;
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
    
    public static async  Task<WebApplication> UseIdentityMiddleware(this WebApplication app)
    {
        app.MapUserEndpoints()
            .MapAuthenticationEndpoints();
        app.UseAuthentication();
        app.UseAuthorization();
        
        if (app.Environment.IsProduction()) return app;

        using var scope = app.Services.CreateScope();
        await using var identityDbContext = scope.ServiceProvider.GetRequiredService<PharmaRepIdentityDbContext>();
        await identityDbContext.Database.MigrateAsync();

        return app;
    }
}