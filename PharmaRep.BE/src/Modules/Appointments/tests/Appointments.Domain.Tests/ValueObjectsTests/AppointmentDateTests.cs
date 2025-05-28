using Appointments.Domain.Exceptions;
using Appointments.Domain.ValueObjects;

namespace Appointments.Domain.Tests.ValueObjectsTests;

public class AppointmentDateTests
{
    [Fact]
    public void AppointmentDate_ValidDate_CreatesAppointmentDate()
    {
        // Arrange
        var validDate = new DateTime(2023, 10, 1);

        // Act
        AppointmentDate appointmentDate = validDate;

        // Assert
        Assert.Equal(validDate, appointmentDate.Value);
    }
    
    [Fact]
    public void AppointmentDate_DefaultDate_ThrowsAppointDateException()
    {
        // Act & Assert
        Assert.Throws<AppointmentDateException>(() =>
        {
            AppointmentDate appointmentDate = default(DateTime);
        });
    }
}