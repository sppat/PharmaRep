using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Mediator;

namespace Shared.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        
        return services;
    }
}