using Appointments.Application.Dtos;

using Identity.Public.Contracts;

namespace Appointments.WebApi.Responses;

public record GetAppointmentResponse(Guid Id,
	DateTimeOffset Start,
	DateTimeOffset End,
	AddressDto Address,
	UserBasicInfo Organizer,
	IEnumerable<UserBasicInfo> Attendees);
