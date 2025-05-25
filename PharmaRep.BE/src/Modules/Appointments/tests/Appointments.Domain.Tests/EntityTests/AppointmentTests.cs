using Appointments.Domain.Entities;
using Appointments.Domain.Exceptions;
using Appointments.Domain.ValueObjects;

namespace Appointments.Domain.Tests.EntityTests;

public class AppointmentTests
{
    [Fact]
    public void Create_ValidData_ReturnsAppointment()
    {
        // Arrange
        var startDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddHours(1);
        const string address = "123 Main St, City, Country";
        var organizerId = Guid.NewGuid();
        var attendeeIds = new AppointmentAttendeeId[]
        {
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        // Act
        var appointment = Appointment.Create(startDate: startDate,
            endDate: endDate,
            address: address,
            organizerId: organizerId,
            attendeeIds: attendeeIds);

        // Assert
        Assert.NotNull(appointment);
        Assert.Equal(startDate, appointment.StartDate);
        Assert.Equal(endDate, appointment.EndDate);
        Assert.Equal(address, appointment.Address);
        Assert.Equal(organizerId, appointment.OrganizerId.Value);
        Assert.Equivalent(attendeeIds, appointment.AttendeeIds);
    }
    
    [Fact]
    public void Create_EndDateBeforeStartDate_ThrowsException()
    {
        // Arrange
        var startDate = DateTime.UtcNow.AddHours(1);
        var endDate = DateTime.UtcNow;
        const string address = "123 Main St, City, Country";
        var organizerId = Guid.NewGuid();
        var attendeeIds = new AppointmentAttendeeId[]
        {
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        // Act & Assert
        Assert.Throws<AppointmentDateException>(() => 
            Appointment.Create(startDate: startDate,
                endDate: endDate,
                address: address,
                organizerId: organizerId,
                attendeeIds: attendeeIds));
    }
    
    [Fact]
    public void Create_EmptyAddress_ThrowsException()
    {
        // Arrange
        var startDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddHours(1);
        var address = string.Empty;
        var organizerId = Guid.NewGuid();
        var attendeeIds = new AppointmentAttendeeId[]
        {
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        // Act & Assert
        Assert.Throws<AppointmentEmptyAddressException>(() => 
            Appointment.Create(startDate: startDate,
                endDate: endDate,
                address: address,
                organizerId: organizerId,
                attendeeIds: attendeeIds));
    }
    
    [Fact]
    public void Create_EmptyOrganizerId_ThrowsException()
    {
        // Arrange
        var startDate = DateTime.UtcNow;
        var endDate = DateTime.UtcNow.AddHours(1);
        const string address = "123 Main St, City, Country";
        var organizerId = Guid.Empty;
        var attendeeIds = new AppointmentAttendeeId[]
        {
            Guid.NewGuid(),
            Guid.NewGuid()
        };

        // Act & Assert
        Assert.Throws<AppointmentEmptyOrganizerIdException>(() => 
            Appointment.Create(startDate: startDate,
                endDate: endDate,
                address: address,
                organizerId: organizerId,
                attendeeIds: attendeeIds));
    }
}