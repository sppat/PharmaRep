using FluentValidation;
using FluentValidation.Results;
using Identity.Application.Features.User.Register;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using Shared.Application.Results;

namespace Identity.Application.Tests.CommandHandlers;

public class RegisterUserCommandHandlerTests
{
    private readonly RegisterUserCommand _command = new(FirstName: "John",
        LastName: "Doe",
        Email: "john@doe.com",
        Password: "P@ssw0rd",
        Roles: ["Doctor"]);
    private readonly Mock<IValidator<RegisterUserCommand>> _validatorMock = new();
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly RegisterUserCommandHandler _sut;

    public RegisterUserCommandHandlerTests()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _sut = new RegisterUserCommandHandler(_validatorMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_ReturnsSuccessResult()
    {
        // Arrange
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(u => u.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);
        
        // Act
        var result = await _sut.HandleAsync(_command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultType.Created, result.Type);
        Assert.Empty(result.Errors);
    }
    
    [Fact]
    public async Task HandleAsync_RequestValidationFailureInCreateUser_ReturnsValidationErrorResult()
    {
        // Arrange
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult([new ValidationFailure()]));
        
        // Act
        var result = await _sut.HandleAsync(_command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultType.ValidationError, result.Type);
    }
    
    [Fact]
    public async Task HandleAsync_FailureIdentityResultInCreateUser_ReturnsValidationErrorResult()
    {
        // Arrange
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Test Error" }));
        
        // Act
        var result = await _sut.HandleAsync(_command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultType.ValidationError, result.Type);
    }
    
    [Fact]
    public async Task HandleAsync_FailureIdentityResultInAddToRoles_ReturnsValidationErrorResult()
    {
        // Arrange
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        
        _userManagerMock.Setup(u => u.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Test Error" }));
        
        // Act
        var result = await _sut.HandleAsync(_command, CancellationToken.None);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultType.ValidationError, result.Type);
    }
}