using Appointments.Domain.Exceptions.Appointment;
using System.IO;

namespace Appointments.Domain.ValueObjects;

public record AppointmentAddressStreet
{
    public string Value { get; private set; }

    private AppointmentAddressStreet() { }

    private AppointmentAddressStreet(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyStreetException(nameof(AppointmentAddressStreet));
        }

        Value = value;
    }
    
    public static implicit operator string(AppointmentAddressStreet street) => street.Value;
    public static implicit operator AppointmentAddressStreet(string street) => new(street);
}
