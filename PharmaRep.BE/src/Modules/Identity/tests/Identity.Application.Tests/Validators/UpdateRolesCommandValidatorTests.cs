using FluentValidation.TestHelper;
using Identity.Application.Features.User.UpdateRoles;
using Identity.Domain.DomainErrors;

namespace Identity.Application.Tests.Validators;

public class UpdateRolesCommandValidatorTests
{
    private readonly UpdateRolesCommandValidator _sut = new();
    
    [Fact]
    public void Validate_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        var command = new UpdateRolesCommand(Guid.NewGuid(), ["Admin", "Doctor"]);

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        Assert.True(result.IsValid);
    }
    
    [Fact]
    public void Validate_InvalidRoles_ReturnsFailure()
    {
        // Arrange
        var command = new UpdateRolesCommand(Guid.NewGuid(), ["Dummy Role"]);

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(IdentityModuleDomainErrors.UserErrors.InvalidRole, result.Errors.Select(e => e.ErrorMessage));
    }
    
    [Fact]
    public void Validate_EmptyId_ReturnsFailure()
    {
        // Arrange
        var command = new UpdateRolesCommand(Guid.Empty, ["Dummy Role"]);

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(IdentityModuleDomainErrors.UserErrors.EmptyId, result.Errors.Select(e => e.ErrorMessage));
    }
}