using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        
        return services;
    }
}