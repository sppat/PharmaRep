using Appointments.Application;
using Appointments.Infrastructure;
using Appointments.Infrastructure.Database;
using Appointments.WebApi.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Appointments.WebApi;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAppointmentsWebApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppointmentsInfrastructure(configuration)
            .AddAppointmentsApplication();

        return services;
    }

    public static async  Task<WebApplication> UseAppointmentMiddleware(this WebApplication app)
    {
        app.MapAppointmentEndpoints();
        app.UseAuthentication();
        app.UseAuthorization();
        
        if (app.Environment.IsProduction()) return app;

        using var scope = app.Services.CreateScope();
        await using var appointmentsDbContext = scope.ServiceProvider.GetRequiredService<PharmaRepAppointmentsDbContext>();
        await appointmentsDbContext.Database.MigrateAsync();

        return app;
    }
}