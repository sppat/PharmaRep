using Appointments.Application.Dtos;

using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Appointments.Application.Features.Appointment.GetAppointment;

public record GetAppointmentQuery(Guid Id) : IRequest<Result<AppointmentDto>>;
