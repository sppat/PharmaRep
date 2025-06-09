using Appointments.Application.Features;
using Appointments.Application.Features.Appointment.Create;
using Appointments.Application.Features.Appointment.Create.Validators;
using Appointments.Application.Features.Appointment.GetAll;
using Identity.Application.Features.User.GetAll;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Mediator;
using Shared.Application.Results;
using Shared.Application.Validation;

namespace Appointments.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAppointmentsApplication(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateAppointmentCommand>, UserIdsExistValidator>();
        services.AddScoped<IValidationOrchestrator<CreateAppointmentCommand>, AppointmentValidator<CreateAppointmentCommand>>();
        
        services.AddScoped<IRequestHandler<CreateAppointmentCommand, Result<Guid>>, CreateAppointmentCommandHandler>();

        services.AddScoped< IRequestHandler<GetAppointmentsQuery, Result<AppointmentsPaginatedResult>>, GetAppointmentsQueryHandler>();

        return services;
    }
}