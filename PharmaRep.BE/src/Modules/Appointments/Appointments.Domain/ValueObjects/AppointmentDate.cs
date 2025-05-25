using Appointments.Domain.DomainErrors;
using Appointments.Domain.Exceptions;

namespace Appointments.Domain.ValueObjects;

public record AppointmentDate
{
    public DateTime Value { get; }
    
    private AppointmentDate(DateTime value)
    {
        if (value < DateTime.UtcNow)
        {
            throw new AppointmentDateException(AppointmentsModuleDomainErrors.AppointmentErrors.PastDate);
        }

        Value = value;
    }
    
    public static implicit operator AppointmentDate(DateTime date) => new(date);
    public static implicit operator DateTime(AppointmentDate appointmentDate) => appointmentDate.Value;
}