using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAppointmentsApplication(this IServiceCollection services)
    {
        return services;
    }
}