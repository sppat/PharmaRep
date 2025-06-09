using Identity.Public.Contracts;

namespace Appointments.Application.Dtos;

public record AppointmentDto(Guid Id, 
    DateTimeOffset Start,
    DateTimeOffset End,
    AddressDto Address,
    UserBasicInfo Organizer,
    IEnumerable<UserBasicInfo> Attendees);