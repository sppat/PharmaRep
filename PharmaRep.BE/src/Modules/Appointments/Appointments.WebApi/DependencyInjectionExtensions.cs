using Appointments.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.WebApi;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAppointmentsWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppointmentsInfrastructure(configuration);

        return services;
    }
}