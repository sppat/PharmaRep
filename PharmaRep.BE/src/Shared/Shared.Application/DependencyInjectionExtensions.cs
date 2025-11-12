using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Mediator;
using Shared.Application.Mediator.Pipeline;
using System.Reflection;

namespace Shared.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDispatcher(this IServiceCollection services)
    {
        services.AddScoped<IDispatcher, Dispatcher>();
        
        return services;
    }
}