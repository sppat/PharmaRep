using Appointments.Domain.DomainErrors;
using Appointments.Domain.Entities;

namespace Appointments.Domain.Tests.EntityTests;

public class AppointmentTests
{
    private const string ValidStreet = "Main St";
    private const ushort ValidNumber = 1;
    private const uint ValidZipCode = 12345;
    
    private readonly DateTimeOffset _validStartDate = DateTimeOffset.UtcNow;
    private readonly DateTimeOffset _validEndDate = DateTimeOffset.UtcNow.AddHours(1);
    private readonly Guid _validOrganizerId = Guid.NewGuid();
    private readonly Guid[] _validAttendeeIds =
    [
        Guid.NewGuid(),
        Guid.NewGuid()
    ];
    
    [Fact]
    public void Create_ValidData_ReturnsSuccessDomainResult()
    {
        // Act
        var result = Appointment.Create(startDate: _validStartDate,
            endDate: _validEndDate,
            street: ValidStreet,
            number: ValidNumber,
            zipCode: ValidZipCode,
            organizerId: _validOrganizerId,
            attendeeIds: _validAttendeeIds);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        
        var appointment = result.Value;
        Assert.NotEqual(Guid.Empty, appointment.Id.Value);
        Assert.Equal(_validStartDate, appointment.Date.StartDate);
        Assert.Equal(_validEndDate, appointment.Date.EndDate);
        Assert.Equal(ValidStreet, appointment.Address.Street);
        Assert.Equal(ValidNumber, appointment.Address.Number);
        Assert.Equal(ValidZipCode, appointment.Address.ZipCode);
        Assert.Equal(_validOrganizerId, appointment.CreatedBy.Value);
        Assert.Equal(_validAttendeeIds, appointment.Attendees.Select(attendee => attendee.UserId.Value).ToArray());
    }
    
    [Fact]
    public void Create_EndDateBeforeStartDate_ReturnFailDomainResult()
    {
        // Arrange
        var endDate = new DateTime(year: 2000, month: 1, day: 1);

        // Act
        var result = Appointment.Create(startDate: _validStartDate,
            endDate: endDate,
            street: ValidStreet,
            number: ValidNumber,
            zipCode: ValidZipCode,
            organizerId: _validOrganizerId,
            attendeeIds: _validAttendeeIds);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.NotNull(result.ErrorMessage);
        Assert.Equal(AppointmentsModuleDomainErrors.AppointmentErrors.InvalidDate, result.ErrorMessage);
    }
    
    [Fact]
    public void Create_EmptyStreet_ReturnFailDomainResult()
    {
        // Arrange
        var street = string.Empty;

        // Act
        var result = Appointment.Create(startDate: _validStartDate,
            endDate: _validEndDate,
            street: street,
            number: ValidNumber,
            zipCode: ValidZipCode,
            organizerId: _validOrganizerId,
            attendeeIds: _validAttendeeIds);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.NotNull(result.ErrorMessage);
        Assert.Equal(AppointmentsModuleDomainErrors.AppointmentErrors.InvalidAddress, result.ErrorMessage);
    }
    
    [Fact]
    public void Create_EmptyZipCode_ReturnFailDomainResult()
    {
        // Arrange
        const uint zipCode = 0u;

        // Act
        var result = Appointment.Create(startDate: _validStartDate,
            endDate: _validEndDate,
            street: ValidStreet,
            number: ValidNumber,
            zipCode: zipCode,
            organizerId: _validOrganizerId,
            attendeeIds: _validAttendeeIds);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.NotNull(result.ErrorMessage);
        Assert.Equal(AppointmentsModuleDomainErrors.AppointmentErrors.InvalidAddress, result.ErrorMessage);
    }
    
    [Fact]
    public void Create_EmptyOrganizerId_ReturnFailDomainResult()
    {
        // Arrange
        var organizerId = Guid.Empty;
        
        // Act
        var result = Appointment.Create(startDate: _validStartDate,
            endDate: _validEndDate,
            street: ValidStreet,
            number: ValidNumber,
            zipCode: ValidZipCode,
            organizerId: organizerId,
            attendeeIds: _validAttendeeIds);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.NotNull(result.ErrorMessage);
        Assert.Equal(AppointmentsModuleDomainErrors.AppointmentErrors.EmptyOrganizerId, result.ErrorMessage);
    }
    
    [Fact]
    public void Create_EmptyAttendeeId_ReturnFailDomainResult()
    {
        // Arrange
        var attendeeIds = new[] { Guid.Empty };

        // Act
        var result = Appointment.Create(startDate: _validStartDate,
            endDate: _validEndDate,
            street: ValidStreet,
            number: ValidNumber,
            zipCode: ValidZipCode,
            organizerId: _validOrganizerId,
            attendeeIds: attendeeIds);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.NotNull(result.ErrorMessage);
        Assert.Equal(AppointmentsModuleDomainErrors.AppointmentErrors.AttendeeEmptyId, result.ErrorMessage);
    }
}