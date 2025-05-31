using Appointments.Domain.DomainErrors;
using Appointments.Domain.ValueObjects;
using Shared.Domain;

namespace Appointments.Domain.Entities;

public class Appointment
{
    public Guid Id { get; private set; }
    public AppointmentDate Date { get; private set; }
    public AppointmentAddress Address { get; private set; }
    public IEnumerable<UserId> AttendeeIds { get; private set; }
    
    public UserId CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public UserId UpdatedBy { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Appointment(AppointmentDate date,
        AppointmentAddress address,
        UserId createdBy,
        IEnumerable<UserId> attendeeIds)
    {
        Id = Guid.NewGuid();
        Date = date;
        Address = address;
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
        AttendeeIds = attendeeIds ?? [];
    }
    
    public static DomainResult<Appointment> Create(DateTime startDate, 
        DateTime endDate,
        string street,
        ushort number,
        uint zipCode,
        Guid organizerId,
        IEnumerable<Guid> attendeeIds)
    {
        var appointmentAddressIsValid = AppointmentAddress.TryCreate(street, number, zipCode, out var address);
        if (!appointmentAddressIsValid)
        {
            return AppointmentsModuleDomainErrors.AppointmentErrors.InvalidAddress;
        }
        
        var appointmentDateIsValid = AppointmentDate.TryCreate(startDate, endDate, out var date);
        if (!appointmentDateIsValid)
        {
            return AppointmentsModuleDomainErrors.AppointmentErrors.InvalidDate;
        }
        
        var organizerIdIsValid = UserId.TryCreate(organizerId, out var organizer);
        if (!organizerIdIsValid)
        {
            return AppointmentsModuleDomainErrors.AppointmentErrors.EmptyOrganizerId;
        }

        var attendees = new List<UserId>();
        foreach (var attendeeId in attendeeIds)
        {
            var attendeeIdIsValid = UserId.TryCreate(attendeeId, out var attendee);
            if (!attendeeIdIsValid) return AppointmentsModuleDomainErrors.AppointmentErrors.AttendeeEmptyId;
            
            attendees.Add(attendee);
        }
        
        return new Appointment(date, address, organizer, attendees);
    }
}