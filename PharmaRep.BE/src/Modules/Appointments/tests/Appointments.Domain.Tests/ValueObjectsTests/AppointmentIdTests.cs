using Appointments.Domain.ValueObjects;

namespace Appointments.Domain.Tests.ValueObjectsTests;

public class AppointmentIdTests
{
    [Fact]
    public void TryCreate_ValidId_CreatesAppointmentId()
    {
        // Arrange
        var validId = Guid.NewGuid();

        // Act
        var appointmentIdCreated = AppointmentId.TryCreate(validId, out var appointmentId);

        // Assert
        Assert.True(appointmentIdCreated);
        Assert.Equal(validId, appointmentId.Value);
    }
    
    [Fact]
    public void TryCreate_EmptyId_ReturnsFalse()
    {
        // Arrange
        var emptyId = Guid.Empty;

        // Act
        var appointmentIdCreated = AppointmentId.TryCreate(emptyId, out var appointmentId);

        // Assert
        Assert.False(appointmentIdCreated);
        Assert.Null(appointmentId);
    }
}