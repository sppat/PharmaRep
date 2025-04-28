using FluentValidation.TestHelper;
using Identity.Application.Features.Auth.Register;
using Identity.Application.Features.User.Register;
using Identity.Domain.DomainErrors;

namespace Identity.Application.Tests.Validators;

public class RegisterUserCommandValidatorTests
{
    private readonly RegisterCommand _validCommand = new(FirstName: "John",
        LastName: "Doe",
        Email: "john@doe.com",
        Password: "P@ssw0rd",
        Roles: ["Doctor"]);
    private readonly RegisterUserCommandValidator _sut = new();

    [Fact]
    public void Validate_ValidCommand_ReturnsValidResult()
    {
        // Act
        var result = _sut.TestValidate(_validCommand);
        
        // Assert
        Assert.True(result.IsValid);
    }

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

    #region Roles

    [Fact]
    public void Validate_EmptyRoles_ReturnsInvalidResult()
    {
        // Arrange
        var command = _validCommand with
        {
            Roles = []
        };
        
        // Act
        var result = _sut.TestValidate(command);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(IdentityModuleDomainErrors.UserErrors.EmptyRoles, result.Errors.Select(e => e.ErrorMessage));
    }
    
    [Fact]
    public void Validate_RolesNotExist_ReturnsInvalidResult()
    {
        // Arrange
        var command = _validCommand with
        {
            Roles = ["Dummy Role One", "Dummy Role Two"]
        };
        
        // Act
        var result = _sut.TestValidate(command);
        
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(IdentityModuleDomainErrors.UserErrors.InvalidRole, result.Errors.Select(e => e.ErrorMessage));
    }

    #endregion
}