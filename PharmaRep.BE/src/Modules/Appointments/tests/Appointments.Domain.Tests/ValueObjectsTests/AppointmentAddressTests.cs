using Appointments.Domain.Exceptions;
using Appointments.Domain.ValueObjects;

namespace Appointments.Domain.Tests.ValueObjectsTests;

public class AppointmentAddressTests
{
    [Fact]
    public void AppointmentAddress_ValidAddress_CreatesAppointmentAddress()
    {
        // Arrange
        var validAddress = "123 Main St, Springfield";

        // Act
        AppointmentAddress appointmentAddress = validAddress;

        // Assert
        Assert.Equal(validAddress, appointmentAddress.Value);
    }

    [Fact]
    public void AppointmentAddress_EmptyAddress_ThrowsAppointmentEmptyAddressException()
    {
        // Act & Assert
        Assert.Throws<AppointmentEmptyAddressException>(() =>
        {
            AppointmentAddress appointmentAddress = string.Empty;
        });
    }
}