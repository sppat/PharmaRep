using Appointments.Domain.ValueObjects;

namespace Appointments.Domain.Tests.ValueObjectsTests;

public class AppointmentDateTests
{
    private readonly DateTime _validStartDate = DateTime.UtcNow;
    private readonly DateTime _validEndDate = DateTime.UtcNow.AddHours(1);
    
    [Fact]
    public void AppointmentDate_ValidDate_CreatesAppointmentDate()
    {
        // Arrange
        // Act
        var appointmentDateCreated = AppointmentDate.TryCreate(startDate: _validStartDate,
            endDate: _validEndDate, 
            out var appointmentDate);

        // Assert
        Assert.True(appointmentDateCreated);
        Assert.Equal(_validStartDate, appointmentDate.StartDate);
        Assert.Equal(_validEndDate, appointmentDate.EndDate);
    }
    
    [Fact]
    public void AppointmentDate_DefaultStartDate_ReturnsFalse()
    {
        // Act
        var appointmentDateCreated = AppointmentDate.TryCreate(startDate: default,
            endDate: _validEndDate,
            out var appointmentDate);
        
        // Assert
        Assert.False(appointmentDateCreated);
        Assert.Null(appointmentDate);
    }
    
    [Fact]
    public void AppointmentDate_DefaultEndDate_ReturnsFalse()
    {
        // Act
        var appointmentDateCreated = AppointmentDate.TryCreate(startDate: _validStartDate,
            endDate: default,
            out var appointmentDate);
        
        // Assert
        Assert.False(appointmentDateCreated);
        Assert.Null(appointmentDate);
    }
    
    [Fact]
    public void AppointmentDate_EndDateBeforeStartDate_ReturnsFalse()
    {
        // Arrange
        var endDate = _validStartDate.AddHours(-1);
        
        // Act
        var appointmentDateCreated = AppointmentDate.TryCreate(startDate: _validStartDate,
            endDate: endDate,
            out var appointmentDate);
        
        // Assert
        Assert.False(appointmentDateCreated);
        Assert.Null(appointmentDate);
    }
}