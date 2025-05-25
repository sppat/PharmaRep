using Identity.Application.Features.User.Delete;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shared.Application.Results;

namespace Identity.Application.Tests.CommandHandlers;

public class DeleteUserCommandHandlerTests
{
    private readonly DeleteUserCommandHandler _sut;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly User _mockUser = User.Create(email: "test@email.com", firstName: "test", lastName: "test");

    public DeleteUserCommandHandlerTests()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        
        _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _sut = new DeleteUserCommandHandler(_userManagerMock.Object);
    }

    [Fact]
    public async Task HandleAsync_UserExists_ReturnsSuccess()
    {
        // Arrange
        _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(_mockUser);
        _userManagerMock.Setup(u => u.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
        
        var command = new DeleteUserCommand(_mockUser.Id);
        
        // Act
        var result = await _sut.HandleAsync(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultType.Deleted, result.Type);
    }

    [Fact]
    public async Task HandleAsync_UserDoesNotExist_ReturnsFailure()
    {
        // Arrange
        _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(default(User));
        
        var command = new DeleteUserCommand(Guid.Empty);
        
        // Act
        var result = await _sut.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultType.NotFound, result.Type);
    }    
    
    [Fact]
    public async Task HandleAsync_DeleteError_ReturnsServerError()
    {
        // Arrange
        const string errorMessage = "An error occurred while deleting the user.";
        _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(_mockUser);
        _userManagerMock.Setup(u => u.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Failed(new IdentityError
        {
            Code = "Error",
            Description = errorMessage
        }));
        var command = new DeleteUserCommand(_mockUser.Id);
        
        // Act
        var result = await _sut.HandleAsync(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultType.ServerError, result.Type);
        Assert.Contains(errorMessage, result.Errors);
    }
}