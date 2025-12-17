using Appointments.Application.Abstractions;
using Appointments.Application.Features.Appointment.GetAll;
using Appointments.Domain.Entities;
using Identity.Public.Contracts;
using Identity.Public.Features.GetUsersBasicInfo;
using Moq;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Appointments.Application.Tests.CommandHandlers;

public class GetAppointmentsQueryHandlerTests
{
    private readonly Mock<IDispatcher> _dispatcherMock = new();
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock = new();
    private readonly GetAppointmentsQueryHandler _sut;
    
    private readonly Guid _organizerOneId = Guid.NewGuid();
    private readonly Guid _organizerTwoId = Guid.NewGuid();
    private readonly Guid[] _attendeeIds = [Guid.NewGuid(), Guid.NewGuid()];
    private const int DefaultPageSize = 10;
    private const int DefaultPageNumber = 1;
    
    public GetAppointmentsQueryHandlerTests()
    {
        _sut = new GetAppointmentsQueryHandler(_dispatcherMock.Object, _appointmentRepositoryMock.Object);
    }

    [Fact(DisplayName = "Verify that handler returns correct response with appointments list")]
    public async Task HandleAsync_ReturnsAppointmentsList()
    {
        // Arrange
        var expectedAppointments = GetTestAppointments();
        var expectedAppointmentsCount = expectedAppointments.Count;
        
        _appointmentRepositoryMock.Setup(mock => mock.GetAllAsync(
            It.IsAny<Guid?>(),
            It.IsAny<DateTimeOffset?>(),
            It.IsAny<DateTimeOffset?>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedAppointments);
        
        _appointmentRepositoryMock.Setup(mock => mock.CountAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedAppointmentsCount);

        _dispatcherMock.Setup(mock => mock.SendAsync(It.IsAny<GetUsersBasicInfoQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IEnumerable<UserBasicInfo>>.Success(GetTestUsersInfo()));

        // Act
        var result = await _sut.HandleAsync(new GetAppointmentsQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result.Value);
        Assert.NotEmpty(result.Value.Items);
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultType.Success, result.Type);
        Assert.Empty(result.Errors);
        
        var appointments = result.Value.Items.ToList();
        Assert.Equal(expectedAppointmentsCount, appointments.Count);
        Assert.Equal(expectedAppointmentsCount, result.Value.Total);
        Assert.Equal(DefaultPageSize, result.Value.PageSize);
        Assert.Equal(DefaultPageNumber, result.Value.PageNumber);
        
        _appointmentRepositoryMock.Verify(mock => mock.GetAllAsync(
            It.IsAny<Guid?>(),
            It.IsAny<DateTimeOffset?>(),
            It.IsAny<DateTimeOffset?>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<CancellationToken>()), Times.Once);
        
        _appointmentRepositoryMock.Verify(mock => mock.CountAsync(It.IsAny<CancellationToken>()), Times.Once);
        
        _dispatcherMock.Verify(mock => mock.SendAsync(
            It.IsAny<GetUsersBasicInfoQuery>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
    
    private ICollection<Appointment> GetTestAppointments()
    {
        var appointmentOne = Appointment.Create(
            startDate: DateTime.Now,
            endDate: DateTime.Now.AddHours(1),
            street: "Test street one",
            number: 1,
            zipCode: 12345,
            organizerId: _organizerOneId,
            attendeeIds: _attendeeIds);
        var appointmentTwo = Appointment.Create(
            startDate: DateTime.Now,
            endDate: DateTime.Now.AddHours(1),
            street: "Test street two",
            number: 2,
            zipCode: 12789,
            organizerId: _organizerTwoId,
            attendeeIds: _attendeeIds);
        var appointmentThree = Appointment.Create(
            startDate: DateTime.Now,
            endDate: DateTime.Now.AddHours(1),
            street: "Test street three",
            number: 1,
            zipCode: 12345,
            organizerId: _organizerOneId,
            attendeeIds: _attendeeIds);
        var appointmentFour = Appointment.Create(
            startDate: DateTime.Now,
            endDate: DateTime.Now.AddHours(1),
            street: "Test street four",
            number: 2,
            zipCode: 12789,
            organizerId: _organizerTwoId,
            attendeeIds: [.._attendeeIds, _organizerOneId]);

        return [appointmentOne, appointmentTwo, appointmentThree, appointmentFour];
    }

    private IEnumerable<UserBasicInfo> GetTestUsersInfo() => 
        [
            new(_organizerOneId, FirstName: "Test", LastName: "Organizer One", Email: "test@organizerone.com"),
            new(_organizerTwoId, FirstName: "Test", LastName: "Organizer Two", Email: "test@organizertwo.com"),
            new(_attendeeIds[0], FirstName: "Attendee", LastName: "One", Email: "attendee@one.com"),
            new(_attendeeIds[1], FirstName: "Attendee", LastName: "Two", Email: "attendee@two.com")
        ];
}