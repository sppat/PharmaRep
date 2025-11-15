using Identity.Public.Contracts;
using Identity.Public.Features.GetUsersBasicInfo;
using Identity.Public.Features.UsersExist;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Public;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddPublicIdentityModule(this IServiceCollection services)
    {
        return services;
    }
}