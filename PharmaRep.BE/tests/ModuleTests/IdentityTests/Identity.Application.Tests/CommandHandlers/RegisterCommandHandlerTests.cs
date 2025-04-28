using Identity.Application.Features.Auth.Register;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shared.Application.Results;

namespace Identity.Application.Tests.CommandHandlers;

public class RegisterCommandHandlerTests
{
    private readonly RegisterCommand _command = new(FirstName: "John",
        LastName: "Doe",
        Email: "john@doe.com",
        Password: "P@ssw0rd");
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly RegisterCommandHandler _sut;

    public RegisterCommandHandlerTests()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _sut = new RegisterCommandHandler(_userManagerMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_ReturnsSuccessResult()
    {
        // Arrange
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        
        // Act
        var result = await _sut.HandleAsync(_command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultType.Created, result.Type);
        Assert.Empty(result.Errors);
    }
    
    [Fact]
    public async Task HandleAsync_FailureIdentityResultInCreateUser_ReturnsValidationErrorResult()
    {
        // Arrange
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Test Error" }));
        
        // Act
        var result = await _sut.HandleAsync(_command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultType.ValidationError, result.Type);
    }
}