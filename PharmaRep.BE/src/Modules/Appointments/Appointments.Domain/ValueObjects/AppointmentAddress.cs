using Appointments.Domain.Exceptions;

namespace Appointments.Domain.ValueObjects;

public record AppointmentAddress
{
    public string Value { get; }

    private AppointmentAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new AppointmentEmptyAddressException();
        }

        Value = value;
    }
    
    public static implicit operator AppointmentAddress(string address) => new(address);
    public static implicit operator string(AppointmentAddress address) => address.Value;
}