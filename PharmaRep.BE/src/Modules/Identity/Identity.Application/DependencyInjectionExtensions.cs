using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        
        return services;
    }
}