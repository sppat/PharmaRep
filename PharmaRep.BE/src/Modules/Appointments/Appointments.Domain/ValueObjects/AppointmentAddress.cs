namespace Appointments.Domain.ValueObjects;

public record AppointmentAddress
{
    public string Street { get; }
    public ushort Number { get; }
    public uint ZipCode { get; }

    private AppointmentAddress(string street, ushort number, uint zipCode)
    {
        Street = street;
        Number = number;
        ZipCode = zipCode;
    }
    
    public static bool TryCreate(string street, ushort number, uint zipCode, out AppointmentAddress address)
    {
        if (string.IsNullOrWhiteSpace(street) || zipCode == 0)
        {
            address = null;
            return false;
        }

        address = new AppointmentAddress(street, number, zipCode);
        return true;
    }
}