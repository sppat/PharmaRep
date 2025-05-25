using Appointments.Domain.ValueObjects;

namespace Appointments.Domain.Entities;

public class Appointment
{
    public Guid Id { get; private set; }
    public AppointmentDate StartDate { get; private set; }
    public AppointmentDate EndDate { get; private set; }
    public AppointmentAddress Address { get; private set; }
    public Guid Organizer { get; private set; }
    public IEnumerable<Guid> Attendees { get; private set; }
}