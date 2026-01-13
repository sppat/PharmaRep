using Appointments.Application.Abstractions;
using Appointments.Application.Dtos;
using Appointments.Application.Features.Appointment.GetAppointment;
using Appointments.Domain.DomainErrors;
using Appointments.Domain.Entities;

using Identity.Public.Contracts;
using Identity.Public.Features.GetUsersBasicInfo;

using Moq;

using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Appointments.Application.Tests.CommandHandlers;

public class GetAppointmentQueryHandlerTests
{
	private readonly Mock<IDispatcher> _dispatcherMock = new();
	private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock = new();
	private readonly GetAppointmentQueryHandler _sut;

	public GetAppointmentQueryHandlerTests()
	{
		_sut = new GetAppointmentQueryHandler(_dispatcherMock.Object, _appointmentRepositoryMock.Object);
	}

	[Fact(DisplayName = "Verify that handler returns correct response when appointment exists")]
	public async Task HandleAsync_ReturnsCorrectResponse_WhenAppointmentExists()
	{
		// Arrange
		var organizerId = Guid.NewGuid();
		var attendeesIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
		var expectedAppointment = Appointment.Create(startDate: DateTimeOffset.UtcNow,
			endDate: DateTimeOffset.UtcNow.AddDays(1),
			street: "test str",
			number: 1,
			zipCode: 11111,
			organizerId: organizerId,
			attendeeIds: attendeesIds);
		var query = new GetAppointmentQuery(Id: Guid.NewGuid());
		var usersInfo = new[]
		{
			new UserBasicInfo(Id: organizerId, FirstName: "Organizer", LastName: "Test", Email: "test@test.com"),
			new UserBasicInfo(Id: attendeesIds.First(), FirstName: "Attendee", LastName: "One", Email: "attendee@one.com")
		};
		var appointment = expectedAppointment;
		var expectedAppointmentDto = new AppointmentDto(Id: appointment!.Id.Value,
			Start: appointment.StartDate,
			End: appointment.EndDate,
			Address: new AddressDto(Street: appointment.Address.Street,
				Number: appointment.Address.Number,
				ZipCode: appointment.Address.ZipCode),
			Organizer: usersInfo.First(),
			Attendees: [usersInfo.Last()]);

		_appointmentRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(expectedAppointment);

		_dispatcherMock.Setup(dispatcher => dispatcher.SendAsync(It.IsAny<GetUsersBasicInfoQuery>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result<IEnumerable<UserBasicInfo>>.Success(usersInfo));

		// Act
		var result = await _sut.HandleAsync(query, CancellationToken.None);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.Equivalent(expectedAppointmentDto, result.Value);
	}

	[Fact(DisplayName = "verify that handler returns not found when appointment does not exist")]
	public async Task HandleAsync_AppointmentDoesNotExist_ReturnsNotFound()
	{
		// Arrange
		var query = new GetAppointmentQuery(Id: Guid.NewGuid());
		const string expectedError = AppointmentsModuleDomainErrors.AppointmentErrors.AppointmentNotFound;

		_appointmentRepositoryMock.Setup(repo =>
				repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync((Appointment)null);

		// Act
		var result = await _sut.HandleAsync(query, CancellationToken.None);

		// Assert
		Assert.False(result.IsSuccess);
		Assert.Equal(ResultType.NotFound, result.Type);
		Assert.Contains(expectedError, result.Errors);
	}
}
