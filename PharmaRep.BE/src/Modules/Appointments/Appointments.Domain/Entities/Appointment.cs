using Appointments.Domain.DomainErrors;
using Appointments.Domain.ValueObjects;
using Shared.Domain;

namespace Appointments.Domain.Entities;

public class Appointment
{
    public AppointmentId Id { get; private set; }
    public AppointmentDate Date { get; private set; }
    public AppointmentAddress Address { get; private set; }
    public IEnumerable<Attendee> Attendees { get; private set; }
    
    public UserId CreatedBy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public UserId UpdatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    private Appointment() { }
    
    private Appointment(AppointmentId id,
        AppointmentDate date,
        AppointmentAddress address,
        UserId createdBy,
        IEnumerable<Attendee> attendees)
    {
        Id = id;
        Date = date;
        Address = address;
        CreatedBy = createdBy;
        CreatedAt = DateTimeOffset.UtcNow;
        Attendees = attendees ?? [];
        UpdatedBy = null;
        UpdatedAt = null;
    }

    public HashSet<Guid> GetOrganizerAndAttendeesId()
    {
        var attendeesId = Attendees?.Select(attendee => attendee.UserId.Value) ?? [];
        var usersId = new HashSet<Guid>(attendeesId);
        usersId.UnionWith([CreatedBy.Value]);

        return usersId;
    }
    
    public static DomainResult<Appointment> Create(DateTimeOffset startDate, 
        DateTimeOffset endDate,
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

        AppointmentId.TryCreate(Guid.NewGuid(), out var appointmentId);
        var attendees = new List<Attendee>();
        foreach (var attendeeId in attendeeIds ?? [])
        {
            var attendeeIdIsValid = UserId.TryCreate(attendeeId, out _);
            if (!attendeeIdIsValid) return AppointmentsModuleDomainErrors.AppointmentErrors.AttendeeEmptyId;
            
            var attendeeResult = Attendee.Create(userId: attendeeId, appointmentId: appointmentId.Value);
            if (!attendeeResult.IsSuccess) return attendeeResult.ErrorMessage;
            
            attendees.Add(attendeeResult.Value);
        }
        
        return new Appointment(appointmentId, date, address, organizer, attendees);
    }
}