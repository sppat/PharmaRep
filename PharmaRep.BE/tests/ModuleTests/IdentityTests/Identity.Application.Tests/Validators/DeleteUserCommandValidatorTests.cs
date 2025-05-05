using FluentValidation.TestHelper;
using Identity.Application.Features.User.Delete;

namespace Identity.Application.Tests.Validators;

public class DeleteUserCommandValidatorTests
{
    private readonly DeleteUserCommandValidator _sut = new();

    [Fact]
    public void Validate_EmptyUserId_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteUserCommand(Guid.Empty);
        
        // Act
        var result = _sut.TestValidate(command);
        
        // Assert
        Assert.False(result.IsValid);
    }
    
    [Fact]
    public void Validate_NonEmptyUserId_ReturnsSuccess()
    {
        // Arrange
        var command = new DeleteUserCommand(Guid.NewGuid());
        
        // Act
        var result = _sut.TestValidate(command);
        
        // Assert
        Assert.True(result.IsValid);
    }
}