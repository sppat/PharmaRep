using Appointments.Domain.Exceptions.Appointment;
using Appointments.Domain.ValueObjects;

namespace Appointments.Domain.Entities;

public class Appointment
{
    public AppointmentId Id { get; private set; }
    public AppointmentDate StartDate { get; private set; }
    public AppointmentDate EndDate { get; private set; }
    public AppointmentAddress Address { get; private set; }
    public IEnumerable<Attendee> Attendees { get; private set; }
    
    public UserId CreatedBy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public UserId UpdatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    private Appointment() { }

    private Appointment(
        Guid appointmentId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        AppointmentAddress address,
        Guid createdBy,
        IEnumerable<Attendee> attendees)
    {
        if (startDate == endDate || startDate > endDate)
        {
            throw new AppointmentStartDateException();
        }

        Id = appointmentId;
        StartDate = startDate;
        EndDate = endDate;
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
    
    public static Appointment Create(DateTimeOffset startDate, 
        DateTimeOffset endDate,
        string street,
        ushort number,
        uint zipCode,
        Guid organizerId,
        IEnumerable<Guid> attendeeIds)
    {
        var appointmentId = Guid.NewGuid();
        var appointmentAddress = AppointmentAddress.Create(street, number, zipCode);
        var attendees = new List<Attendee>();
        foreach (var attendeeId in attendeeIds ?? [])
        {
            var attendee = Attendee.Create(userId: attendeeId, appointmentId: appointmentId);
            
            attendees.Add(attendee);
        }
        
        return new Appointment(
            appointmentId: appointmentId, 
            startDate: startDate, 
            endDate: endDate, 
            address: appointmentAddress, 
            createdBy: organizerId, 
            attendees: attendees);
    }
}