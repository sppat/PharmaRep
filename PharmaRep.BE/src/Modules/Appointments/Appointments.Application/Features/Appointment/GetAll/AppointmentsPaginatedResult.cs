using Appointments.Application.Dtos;

namespace Appointments.Application.Features.Appointment.GetAll;

public record AppointmentsPaginatedResult(int PageNumber,
	int PageSize,
	int Total,
	IEnumerable<AppointmentDto> Items);
