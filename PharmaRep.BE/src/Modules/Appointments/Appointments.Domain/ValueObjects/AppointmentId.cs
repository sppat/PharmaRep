using Appointments.Domain.Exceptions.Appointment;

namespace Appointments.Domain.ValueObjects;

public record AppointmentId
{
    public Guid Value { get; private set; }
    
    private AppointmentId() { }

    public AppointmentId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new EmptyAppointmentIdException(nameof(AppointmentId));
        }

        Value = value;
    }
    
    public static implicit operator Guid(AppointmentId appointmentId) => appointmentId.Value;
    public static implicit operator AppointmentId(Guid value) => new(value);
}