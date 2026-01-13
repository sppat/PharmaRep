namespace Appointments.Domain.ValueObjects;

public record AppointmentAddress
{
	public AppointmentAddressStreet Street { get; private set; }
	public ushort Number { get; private set; }
	public AppointmentAddressZipCode ZipCode { get; private set; }

	private AppointmentAddress() { }

	private AppointmentAddress(string street, ushort number, uint zipCode)
	{
		Street = street;
		Number = number;
		ZipCode = zipCode;
	}

	public static AppointmentAddress Create(string street, ushort number, uint zipCode) => new(street, number, zipCode);
}
