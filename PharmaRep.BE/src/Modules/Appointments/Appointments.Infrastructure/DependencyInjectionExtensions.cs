﻿using Appointments.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Constants;

namespace Appointments.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAppointmentsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PharmaRepAppointmentsDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(EfConstants.DefaultConnection), builder =>
            {
                builder.MigrationsHistoryTable(EfConstants.MigrationsHistoryTable, EfConstants.Schemas.Appointments);
            });
        });

        return services;
    }
}