using Appointments.Domain.Exceptions;
using Appointments.Domain.ValueObjects;

namespace Appointments.Domain.Tests.ValueObjectsTests;

public class AppointmentAttendeeIdTests
{
    [Fact]
    public void AppointmentAttendeeId_ValidId_CreatesAppointmentAttendeeId()
    {
        // Arrange
        var validId = Guid.NewGuid();

        // Act
        AppointmentAttendeeId appointmentAttendeeId = validId;

        // Assert
        Assert.Equal(validId, appointmentAttendeeId.Value);
    }
    
    [Fact]
    public void AppointmentAttendeeId_DefaultId_ThrowsAppointmentAttendeeIdException()
    {
        // Act & Assert
        Assert.Throws<AppointmentEmptyAttendeeIdException>(() =>
        {
            AppointmentAttendeeId appointmentAttendeeId = Guid.Empty;
        });
    }
}