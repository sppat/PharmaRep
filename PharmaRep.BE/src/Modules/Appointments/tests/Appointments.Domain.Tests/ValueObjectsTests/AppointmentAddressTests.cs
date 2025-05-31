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
        var appointmentAddressCreated = AppointmentAddress.TryCreate(street: ValidStreet, 
            number: ValidNumber,
            zipCode: ValidZipCode,
            out var appointmentAddress);

        // Assert
        Assert.True(appointmentAddressCreated);
        Assert.Equal(ValidStreet, appointmentAddress.Street);
        Assert.Equal(ValidNumber, appointmentAddress.Number);
        Assert.Equal(ValidZipCode, appointmentAddress.ZipCode);
    }

    [Fact]
    public void AppointmentAddress_EmptyAddress_ReturnsFalse()
    {
        // Act
        var appointmentAddressCreated = AppointmentAddress.TryCreate(street: string.Empty, 
            number: ValidNumber,
            zipCode: ValidZipCode,
            out var appointmentAddress);

        // Assert
        Assert.False(appointmentAddressCreated);
        Assert.Null(appointmentAddress);
    }
    
    [Fact]
    public void AppointmentAddress_ZeroZipCode_ReturnsFalse()
    {
        // Act
        var appointmentAddressCreated = AppointmentAddress.TryCreate(street: ValidStreet, 
            number: ValidNumber,
            zipCode: 0,
            out var appointmentAddress);

        // Assert
        Assert.False(appointmentAddressCreated);
        Assert.Null(appointmentAddress);
    }
}