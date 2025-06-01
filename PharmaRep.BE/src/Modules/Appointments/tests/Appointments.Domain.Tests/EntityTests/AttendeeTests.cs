using Appointments.Domain.DomainErrors;
using Appointments.Domain.Entities;

namespace Appointments.Domain.Tests.EntityTests;

public class AttendeeTests
{
    [Fact]
    public void Create_ValidUserAndAppointmentId_CreatesAttendee()
    {
        // Arrange
        var validUserId = Guid.NewGuid();
        var appointmentId = Guid.NewGuid();

        // Act
        var attendeeResult = Attendee.Create(validUserId, appointmentId);

        // Assert
        Assert.True(attendeeResult.IsSuccess);
        
        var attendee = attendeeResult.Value;
        Assert.Equal(validUserId, attendee?.UserId.Value);
        Assert.Equal(appointmentId, attendee?.AppointmentId.Value);
    }
    
    [Fact]
    public void Create_EmptyUserId_ReturnsError()
    {
        // Arrange
        var emptyUserId = Guid.Empty;
        var appointmentId = Guid.NewGuid();

        // Act
        var attendeeResult = Attendee.Create(emptyUserId, appointmentId);

        // Assert
        Assert.False(attendeeResult.IsSuccess);
        Assert.Null(attendeeResult.Value);
        Assert.Equal(AppointmentsModuleDomainErrors.AppointmentErrors.AttendeeEmptyId, attendeeResult.ErrorMessage);
    }
    
    [Fact]
    public void Create_EmptyAppointmentId_ReturnsError()
    {
        // Arrange
        var validUserId = Guid.NewGuid();
        var emptyAppointmentId = Guid.Empty;

        // Act
        var attendeeResult = Attendee.Create(validUserId, emptyAppointmentId);

        // Assert
        Assert.False(attendeeResult.IsSuccess);
        Assert.Null(attendeeResult.Value);
        Assert.Equal(AppointmentsModuleDomainErrors.AppointmentErrors.AppointmentEmptyId, attendeeResult.ErrorMessage);
    }
}