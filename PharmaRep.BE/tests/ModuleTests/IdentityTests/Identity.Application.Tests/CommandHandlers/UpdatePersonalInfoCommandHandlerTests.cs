using Identity.Application.Features.User.UpdatePersonalInfo;
using Identity.Application.Interfaces;
using Identity.Domain.DomainErrors;
using Identity.Domain.Entities;
using Identity.Domain.Exceptions.UserExceptions;
using Moq;
using Shared.Application.Results;
using Shared.Tests.Database;

namespace Identity.Application.Tests.CommandHandlers;

public class UpdatePersonalInfoCommandHandlerTests
{
    private readonly UpdatePersonalInfoCommandHandler _sut;
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IIdentityUnitOfWork> _identityUnitOfWorkMock = new();
    private readonly UpdatePersonalInfoCommand _updatePersonalInfoCommand = new(Guid.NewGuid(), "John", "Doe");
    private readonly User _mockUser = User.Create(email: "test@user.com", firstName: "John", lastName: "Doe");
    
    public UpdatePersonalInfoCommandHandlerTests()
    {
        _sut = new UpdatePersonalInfoCommandHandler(_userRepositoryMock.Object, _identityUnitOfWorkMock.Object);
    }

    [Fact]
    public async Task HandleAsync_UserNotExist_ReturnNotFoundResult()
    {
        // Arrange
        _userRepositoryMock.Setup(u => u.GetUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(User));
        var expectedError = IdentityModuleDomainErrors.UserErrors.UserNotFound;
        
        // Act
        var result = await _sut.HandleAsync(_updatePersonalInfoCommand, CancellationToken.None);
        
        // Assert
        Assert.Equal(ResultType.NotFound, result.Type);
        Assert.Contains(expectedError, result.Errors);
    }

    [Fact]
    public async Task HandleAsync_SameFirstAndLastName_DoNotReachDatabase()
    {
        // Arrange
        _userRepositoryMock.Setup(u => u.GetUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockUser);
        
        // Act
        var result = await _sut.HandleAsync(_updatePersonalInfoCommand, CancellationToken.None);
        
        // Assert
        Assert.Equal(ResultType.Updated, result.Type);
        _identityUnitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    [InlineData("1abc")]
    public async Task HandleAsync_InvalidFirstName_ThrowsException(string firstName)
    {
        // Arrange
        _userRepositoryMock.Setup(u => u.GetUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockUser);
        var command = _updatePersonalInfoCommand with { FirstName = firstName };
        
        // Act
        var action = async () => await _sut.HandleAsync(command, CancellationToken.None);
        
        // Assert
        await Assert.ThrowsAsync<UserArgumentException>(action);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    [InlineData("1abc")]
    public async Task HandleAsync_InvalidLastName_ThrowsException(string lastName)
    {
        // Arrange
        _userRepositoryMock.Setup(u => u.GetUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockUser);
        var command = _updatePersonalInfoCommand with { LastName = lastName };
        
        // Act
        var action = async () => await _sut.HandleAsync(command, CancellationToken.None);
        
        // Assert
        await Assert.ThrowsAsync<UserArgumentException>(action);
    }
    
    [Fact]
    public async Task HandleAsync_ValidCommand_ReturnsUpdated()
    {
        // Arrange
        _userRepositoryMock.Setup(u => u.GetUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockUser);
        var command = new UpdatePersonalInfoCommand(_mockUser.Id, FirstName: "Hello", LastName: "World");
        
        // Act
        var result = await _sut.HandleAsync(command, CancellationToken.None);
        
        // Assert
        Assert.Equal(ResultType.Updated, result.Type);
        _identityUnitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}