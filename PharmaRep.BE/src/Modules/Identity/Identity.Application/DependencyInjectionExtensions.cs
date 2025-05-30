using System.Reflection;
using FluentValidation;
using Identity.Application.Dtos;
using Identity.Application.Features.Auth.Login;
using Identity.Application.Features.Auth.Register;
using Identity.Application.Features.User.Delete;
using Identity.Application.Features.User.GetAll;
using Identity.Application.Features.User.GetById;
using Identity.Application.Features.User.UpdatePersonalInfo;
using Identity.Application.Features.User.UpdateRoles;
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
        services.AddScoped<IRequestHandler<RegisterCommand, Result<Guid>>, RegisterCommandHandler>();
        services.AddScoped<IRequestHandler<LoginCommand, Result<string>>, LoginCommandHandler>();
        services.AddScoped<IRequestHandler<UpdateRolesCommand, Result>, UpdateRolesCommandHandler>();
        services.AddScoped<IRequestHandler<DeleteUserCommand, Result>, DeleteUserCommandHandler>();
        services.AddScoped<IRequestHandler<UpdatePersonalInfoCommand, Result>, UpdatePersonalInfoCommandHandler>();

        // Queries
        services.AddScoped<IRequestHandler<GetUserByIdQuery, Result<UserDto>>, GetUserByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetAllUsersQuery, Result<UsersPaginatedResult>>, GetAllUsersQueryHandler>();
        
        return services;
    }
}