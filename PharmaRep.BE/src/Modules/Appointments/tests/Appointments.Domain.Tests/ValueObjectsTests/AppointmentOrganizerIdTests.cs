using Appointments.Domain.Exceptions;
using Appointments.Domain.ValueObjects;

namespace Appointments.Domain.Tests.ValueObjectsTests;

public class AppointmentOrganizerIdTests
{
    [Fact]
    public void AppointmentOrganizerId_ValidId_CreatesAppointmentOrganizerId()
    {
        // Arrange
        var validId = Guid.NewGuid();

        // Act
        AppointmentOrganizerId appointmentOrganizerId = validId;

        // Assert
        Assert.Equal(validId, appointmentOrganizerId.Value);
    }
    
    [Fact]
    public void AppointmentOrganizerId_DefaultId_ThrowsAppointmentOrganizerIdException()
    {
        // Act & Assert
        Assert.Throws<AppointmentEmptyOrganizerIdException>(() =>
        {
            AppointmentOrganizerId appointmentOrganizerId = Guid.Empty;
        });
    }
}