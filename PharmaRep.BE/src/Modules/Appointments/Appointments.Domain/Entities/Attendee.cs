using Appointments.Domain.ValueObjects;

namespace Appointments.Domain.Entities;

public class Attendee
{
    public UserId UserId { get; private set; }
    public AppointmentId AppointmentId { get; private set; }
    
    private Attendee() { }

    private Attendee(UserId userId, AppointmentId appointmentId)
    {
        UserId = userId;
        AppointmentId = appointmentId;
    }

    public static Attendee Create(Guid userId, Guid appointmentId) => new(userId: userId, appointmentId: appointmentId);
}