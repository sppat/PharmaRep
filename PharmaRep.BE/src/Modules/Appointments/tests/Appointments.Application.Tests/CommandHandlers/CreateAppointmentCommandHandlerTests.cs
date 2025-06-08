using Appointments.Application.Abstractions;
using Appointments.Application.Features.Appointment.Create;
using Appointments.Domain.DomainErrors;
using Moq;
using Shared.Application.Results;

namespace Appointments.Application.Tests.CommandHandlers;

public class CreateAppointmentCommandHandlerTests
{
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock = new();
    private readonly Mock<IAppointmentUnitOfWork> _unitOfWorkMock = new();
    private readonly CreateAppointmentCommandHandler _sut;
    private readonly CreateAppointmentCommand _validCommand = new(StartDate: DateTime.UtcNow.AddDays(1),
        EndDate: DateTime.UtcNow.AddDays(2),
        Street: "Main St",
        Number: 456,
        ZipCode: 78910,
        OrganizerId: Guid.NewGuid(),
        AttendeeIds: new List<Guid> { Guid.NewGuid(), Guid.NewGuid() });

    public CreateAppointmentCommandHandlerTests()
    {
        _sut = new CreateAppointmentCommandHandler(_appointmentRepositoryMock.Object, _unitOfWorkMock.Object);
    }
    
    [Fact(DisplayName = "Verify that HandleAsync method returns success result for valid command")]
    public async Task HandleAsync_ValidCommand_ReturnsSuccessResult()
    {
        // Act
        var result = await _sut.HandleAsync(_validCommand, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
        
        _appointmentRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Domain.Entities.Appointment>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact(DisplayName = "Verify that HandleAsync method returns failure result for invalid command")]
    public async Task HandleAsync_InvalidCommand_ReturnsFailureResult()
    {
        // Arrange
        var invalidCommand = _validCommand with { OrganizerId = Guid.Empty };
        const string expectedError = AppointmentsModuleDomainErrors.AppointmentErrors.EmptyOrganizerId;

        // Act
        var result = await _sut.HandleAsync(invalidCommand, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultType.ValidationError, result.Type);
        Assert.Contains(expectedError, result.Errors);
        
        _appointmentRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Domain.Entities.Appointment>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}