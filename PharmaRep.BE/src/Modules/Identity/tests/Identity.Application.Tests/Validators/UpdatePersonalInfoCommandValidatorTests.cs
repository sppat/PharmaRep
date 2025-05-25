using FluentValidation.TestHelper;
using Identity.Application.Features.User.UpdatePersonalInfo;
using Identity.Domain.DomainErrors;

namespace Identity.Application.Tests.Validators;

public class UpdatePersonalInfoCommandValidatorTests
{
    private readonly UpdatePersonalInfoCommandValidator _sut = new();
    private readonly UpdatePersonalInfoCommand _validCommand = new(Guid.NewGuid(), FirstName: "John", LastName: "Doe");

    #region Id

    [Fact]
    public void Validate_IdIsEmpty_ReturnsInvalidResult()
    {
        // Arrange
        var command = _validCommand with
        {
            UserId = Guid.Empty
        };

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(IdentityModuleDomainErrors.UserErrors.EmptyId, result.Errors.Select(e => e.ErrorMessage));
    }

    #endregion
    
    #region FirstName

    [Fact]
    public void Validate_FirstNameExceedsMaxLength_ReturnsInvalidResult()
    {
        // Arrange
        var command = _validCommand with
        {
            FirstName = string.Concat(Enumerable.Repeat("a", 51))
        };

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(IdentityModuleDomainErrors.UserErrors.NameOutOfRange, result.Errors.Select(e => e.ErrorMessage));
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    [InlineData("123")]
    [InlineData("43ac3")]
    [InlineData("!abc")]
    public void Validate_InvalidFirstName_ReturnsInvalidResult(string firstName)
    {
        // Arrange
        var command = _validCommand with
        {
            FirstName = firstName
        };

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(IdentityModuleDomainErrors.UserErrors.InvalidFirstName, result.Errors.Select(e => e.ErrorMessage));
    }

    #endregion

    #region LastName

    [Fact]
    public void Validate_LastNameExceedsMaxLength_ReturnsInvalidResult()
    {
        // Arrange
        var command = _validCommand with
        {
            LastName = string.Concat(Enumerable.Repeat("a", 51))
        };

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(IdentityModuleDomainErrors.UserErrors.NameOutOfRange, result.Errors.Select(e => e.ErrorMessage));
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    [InlineData("123")]
    [InlineData("43ac3")]
    [InlineData("!abc")]
    public void Validate_InvalidLastName_ReturnsInvalidResult(string lastName)
    {
        // Arrange
        var command = _validCommand with
        {
            LastName = lastName
        };

        // Act
        var result = _sut.TestValidate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(IdentityModuleDomainErrors.UserErrors.InvalidLastName, result.Errors.Select(e => e.ErrorMessage));
    }

    #endregion
}