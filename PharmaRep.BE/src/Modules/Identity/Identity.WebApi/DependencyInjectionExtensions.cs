using Identity.Infrastructure;
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
}