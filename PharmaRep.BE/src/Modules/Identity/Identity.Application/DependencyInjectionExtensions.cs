using System.Reflection;
using FluentValidation;
using Identity.Application.Features.User.Register;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddScoped<IRequestHandler<RegisterUserCommand, Result<Guid>>, RegisterUserCommandHandler>();
        
        return services;
    }
}