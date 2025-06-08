using Appointments.Application.Features.Appointment.Create;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Appointments.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAppointmentsApplication(this IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<CreateAppointmentCommand, Result<Guid>>, CreateAppointmentCommandHandler>();

        return services;
    }
}