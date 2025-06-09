using Appointments.Application.Dtos;
using Appointments.Domain.Entities;
using Identity.Public.Contracts;

namespace Appointments.Application.Mappings;

public static class AppointmentMappings
{
    public static AppointmentDto ToDto(this Appointment appointment, UserBasicInfo organizer, IEnumerable<UserBasicInfo> attendees)
    {
        var addressDto = new AddressDto(Street: appointment.Address.Street,
            Number: appointment.Address.Number,
            ZipCode: appointment.Address.ZipCode);
        
        return new AppointmentDto(Id: appointment.Id.Value,
            Start: appointment.Date.StartDate,
            End: appointment.Date.EndDate,
            Address: addressDto,
            Organizer: organizer,
            Attendees: attendees ?? []);
    }
}