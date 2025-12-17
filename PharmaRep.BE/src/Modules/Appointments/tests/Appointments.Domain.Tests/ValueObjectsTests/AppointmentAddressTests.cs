using Appointments.Domain.Exceptions.Appointment;
using Appointments.Domain.ValueObjects;

namespace Appointments.Domain.Tests.ValueObjectsTests;

public class AppointmentAddressTests
{
    private const string ValidStreet = "Main St";
    private const ushort ValidNumber = 123;
    private const uint ValidZipCode = 12345;

    [Fact]
    public void AppointmentAddress_ValidAddress_CreatesAppointmentAddress()
    {
        // Act
        var appointmentAddress = AppointmentAddress.Create(street: ValidStreet, 
            number: ValidNumber,
            zipCode: ValidZipCode);

        // Assert
        Assert.Equal(ValidStreet, appointmentAddress.Street);
        Assert.Equal(ValidNumber, appointmentAddress.Number);
        Assert.Equal(ValidZipCode, appointmentAddress.ZipCode.Value);
    }

    [Fact]
    public void AppointmentAddress_EmptyAddress_ReturnsFalse()
    {
        // Act & Assert
        Assert.Throws<EmptyStreetException>(() => AppointmentAddress.Create(
            street: string.Empty,
            number: ValidNumber,
            zipCode: ValidZipCode));
    }
    
    [Fact]
    public void AppointmentAddress_ZeroZipCode_ReturnsFalse()
    {
        // Act & Assert
        Assert.Throws<InvalidZipCodeException>(() => AppointmentAddress.Create(
            street: ValidStreet,
            number: ValidNumber,
            zipCode: 0));
    }
}