using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Appointments.Application.Features.Appointment.GetAll;

public record GetAppointmentsQuery(Guid? UserId = null,
    DateTimeOffset? From = null,
    DateTimeOffset? To = null,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<Result<AppointmentsPaginatedResult>>;