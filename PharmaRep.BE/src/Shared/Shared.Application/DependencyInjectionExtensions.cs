using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Mediator;
using Shared.Application.Mediator.Pipeline;
using Shared.Application.Validation;

namespace Shared.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDispatcher(this IServiceCollection services)
    {
        services.AddScoped<HandlerWrapperFactory>();
        services.AddScoped<IDispatcher, Dispatcher>();
        
        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>))
                .Where(type => !type.IsGenericTypeDefinition || type != typeof(ValidationDecorator<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(classes => classes.AssignableTo(typeof(IValidationOrchestrator<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        services.Decorate(typeof(IRequestHandler<,>), typeof(ValidationDecorator<,>));
        
        return services;
    }
}