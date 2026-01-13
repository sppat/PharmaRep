using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Appointments.Application.Features.Appointment.Create;

public record CreateAppointmentCommand(DateTimeOffset StartDate,
	DateTimeOffset EndDate,
	string Street,
	ushort Number,
	uint ZipCode,
	Guid OrganizerId,
	IEnumerable<Guid> AttendeeIds) : IRequest<Result<Guid>>;
