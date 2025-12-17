using Appointments.Domain.DomainErrors;
using Appointments.Domain.Entities;
using Appointments.Domain.Exceptions.Appointment;

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
    public void Create_ValidData_ReturnsAppointment()
    {
        // Act
        var appointment = Appointment.Create(startDate: _validStartDate,
            endDate: _validEndDate,
            street: ValidStreet,
            number: ValidNumber,
            zipCode: ValidZipCode,
            organizerId: _validOrganizerId,
            attendeeIds: _validAttendeeIds);

        // Assert
        Assert.NotNull(appointment);
        
        Assert.NotEqual(Guid.Empty, appointment.Id.Value);
        Assert.Equal(_validStartDate, appointment.StartDate.Value);
        Assert.Equal(_validEndDate, appointment.EndDate.Value);
        Assert.Equal(ValidStreet, appointment.Address.Street);
        Assert.Equal(ValidNumber, appointment.Address.Number);
        Assert.Equal(ValidZipCode, appointment.Address.ZipCode.Value);
        Assert.Equal(_validOrganizerId, appointment.CreatedBy.Value);
        Assert.Equal(_validAttendeeIds, appointment.Attendees.Select(attendee => attendee.UserId.Value).ToArray());
    }
    
    [Fact]
    public void Create_EndDateBeforeStartDate_ThrowsAppointmentStartDateException()
    {
        // Arrange
        var endDate = new DateTime(year: 2000, month: 1, day: 1);

        // Act & Assert
        Assert.Throws<AppointmentStartDateException>(() =>
            Appointment.Create(
                startDate: _validStartDate,
                endDate: endDate,
                street: ValidStreet,
                number: ValidNumber,
                zipCode: ValidZipCode,
                organizerId: _validOrganizerId,
                attendeeIds: _validAttendeeIds));
    }
    
    [Fact]
    public void Create_EmptyStreet_ReturnFailDomainResult()
    {
        // Arrange
        var street = string.Empty;

        // Act & Assert
        Assert.Throws<EmptyStreetException>(() => 
            Appointment.Create(startDate: _validStartDate,
                endDate: _validEndDate,
                street: street,
                number: ValidNumber,
                zipCode: ValidZipCode,
                organizerId: _validOrganizerId,
                attendeeIds: _validAttendeeIds));
    }
    
    [Fact]
    public void Create_EmptyZipCode_ReturnFailDomainResult()
    {
        // Arrange
        const uint zipCode = 0u;

        // Act & Assert
        Assert.Throws<InvalidZipCodeException>(() => 
            Appointment.Create(startDate: _validStartDate,
                endDate: _validEndDate,
                street: ValidStreet,
                number: ValidNumber,
                zipCode: zipCode,
                organizerId: _validOrganizerId,
                attendeeIds: _validAttendeeIds));
    }
    
    [Fact]
    public void Create_EmptyOrganizerId_ReturnFailDomainResult()
    {
        // Arrange
        var organizerId = Guid.Empty;

        // Act & Assert
        Assert.Throws<EmptyUserIdException>(() =>
            Appointment.Create(startDate: _validStartDate,
                endDate: _validEndDate,
                street: ValidStreet,
                number: ValidNumber,
                zipCode: ValidZipCode,
                organizerId: organizerId,
                attendeeIds: _validAttendeeIds));
    }
    
    [Fact]
    public void Create_EmptyAttendeeId_ReturnFailDomainResult()
    {
        // Arrange
        var attendeeIds = new[] { Guid.Empty };

        // Act & Assert
        Assert.Throws<EmptyUserIdException>(() =>
            Appointment.Create(startDate: _validStartDate,
                endDate: _validEndDate,
                street: ValidStreet,
                number: ValidNumber,
                zipCode: ValidZipCode,
                organizerId: _validOrganizerId,
                attendeeIds: attendeeIds));
    }
}