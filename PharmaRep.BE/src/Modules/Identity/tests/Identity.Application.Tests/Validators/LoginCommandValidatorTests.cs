using FluentValidation.TestHelper;
using Identity.Application.Features.Auth.Login;
using Identity.Domain.DomainErrors;

namespace Identity.Application.Tests.Validators;

public class LoginCommandValidatorTests
{
    private readonly LoginCommand _validCommand = new(Email: "john@doe.com", Password: "P@ssw0rd");
    private readonly LoginCommandValidator _sut = new();

    [Fact]
    public void Validate_ValidCommand_ReturnsValidResult()
    {
        // Act
        var result = _sut.TestValidate(_validCommand);
        
        // Assert
        Assert.True(result.IsValid);
    }
    
    #region Email

    [Fact]
    public void Validate_EmailExceedsMaxLength_ReturnsInvalidResult()
    {
        // Arrange
        var command = _validCommand with
        {
            Email = string.Concat(Enumerable.Repeat("a", 101))
        };
        
        // Act
        var result = _sut.TestValidate(command);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(IdentityModuleDomainErrors.UserErrors.EmailOutOfRange, result.Errors.Select(e => e.ErrorMessage));
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    [InlineData("123")]
    [InlineData("43ac3")]
    [InlineData("!abc")]
    [InlineData("! abc")]
    [InlineData("abc@ .com")]
    public void Validate_InvalidEmail_ReturnsInvalidResult(string email)
    {
        // Arrange
        var command = _validCommand with
        {
            Email = email
        };
        
        // Act
        var result = _sut.TestValidate(command);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(IdentityModuleDomainErrors.UserErrors.InvalidEmail, result.Errors.Select(e => e.ErrorMessage));
    }

    #endregion
    
    #region Password
    
    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Validate_InvalidPassword_ReturnsInvalidResult(string password)
    {
        // Arrange
        var command = _validCommand with
        {
            Password = password
        };
        
        // Act
        var result = _sut.TestValidate(command);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(IdentityModuleDomainErrors.UserErrors.InvalidPassword, result.Errors.Select(e => e.ErrorMessage));
    }

    #endregion
}