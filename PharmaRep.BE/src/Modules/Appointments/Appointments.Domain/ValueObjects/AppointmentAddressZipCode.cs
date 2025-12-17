using Appointments.Domain.Exceptions.Appointment;

namespace Appointments.Domain.ValueObjects;

public record AppointmentAddressZipCode
{
    public uint Value { get; private set; }

    public AppointmentAddressZipCode(uint value)
    {
        if (value is 0)
        {
            throw new InvalidZipCodeException(nameof(AppointmentAddressZipCode));
        }

        Value = value;
    }

    public static implicit operator uint(AppointmentAddressZipCode zipCode) => zipCode.Value;
    public static implicit operator AppointmentAddressZipCode(uint zipCode) => new(zipCode);
}
