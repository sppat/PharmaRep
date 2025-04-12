using Identity.Application.Features.User.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Constants;

namespace Identity.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PharmaRepIdentityDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"), builder =>
            {
                builder.MigrationsHistoryTable(EfConstants.MigrationsHistoryTable, EfConstants.Schemas.Identity);
            });
        });

        services.AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
            })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<PharmaRepIdentityDbContext>();

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}