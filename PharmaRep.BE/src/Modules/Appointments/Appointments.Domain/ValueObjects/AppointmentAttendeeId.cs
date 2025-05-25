using Appointments.Domain.Exceptions;

namespace Appointments.Domain.ValueObjects;

public record AppointmentAttendeeId
{
    public Guid Value { get; init; }

    private AppointmentAttendeeId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new AppointmentEmptyAttendeeIdException();
        }
        
        Value = value;
    }
    
    public static implicit operator AppointmentAttendeeId(Guid attendeeIds) => new(attendeeIds);
    public static implicit operator Guid(AppointmentAttendeeId appointmentAttendeeId) => appointmentAttendeeId.Value;
}