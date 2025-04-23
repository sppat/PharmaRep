using System.Reflection;
using FluentValidation;
using Identity.Application.Dtos;
using Identity.Application.Features.User.GetAll;
using Identity.Application.Features.User.GetById;
using Identity.Application.Features.User.Login;
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
        
        // Commands
        services.AddScoped<IRequestHandler<RegisterUserCommand, Result<Guid>>, RegisterUserCommandHandler>();
        services.AddScoped<IRequestHandler<LoginUserCommand, Result<string>>, LoginUserCommandHandler>();

        // Queries
        services.AddScoped<IRequestHandler<GetUserByIdQuery, Result<UserDto>>, GetUserByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetAllUsersQuery, Result<UsersPaginatedResult>>, GetAllUsersQueryHandler>();
        
        return services;
    }
}