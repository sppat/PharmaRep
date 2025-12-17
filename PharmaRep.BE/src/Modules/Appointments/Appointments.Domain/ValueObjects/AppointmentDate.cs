using Appointments.Domain.Exceptions.Appointment;

namespace Appointments.Domain.ValueObjects;

public record AppointmentDate
{
    public DateTimeOffset Value { get; private set; }
    
    public AppointmentDate(DateTimeOffset value)
    {
        if (value == default)
        {
            throw new EmptyAppointmentDateException(nameof(AppointmentDate));
        }

        Value = value;
    }

    public static implicit operator DateTimeOffset(AppointmentDate appointmentDate) => appointmentDate.Value;
    public static implicit operator AppointmentDate(DateTimeOffset date) => new(date);
}