using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Identity.Domain.DomainErrors;
using Identity.Domain.Entities;
using Identity.WebApi.Endpoints;
using Identity.WebApi.Requests;
using Identity.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;
using Shared.Tests;

namespace Identity.WebApi.Tests.Endpoints;

[Collection(name: SharedTestConstants.WebApplicationCollectionName)]
public class UserEndpointsTests(WebApplicationFixture fixture) : IClassFixture<WebApplicationFixture>
{
    private readonly HttpClient _httpClient = fixture.CreateClient();

    #region Register

    [Fact]
    public async Task RegisterUser_ValidRequest_ReturnSuccessResponse()
    {
        // Act
        var response = await _httpClient.PostAsJsonAsync(IdentityModuleUrls.User.Register, TestEnvironment.ValidRegisterUserRequest);
        var responseContent = await response.Content.ReadFromJsonAsync<RegisterUserResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotEqual(Guid.Empty, responseContent.UserId);
    }
    
    #region FirstName
    
    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    [InlineData("12a")]
    public async Task RegisterUser_InvalidFirstName_ReturnBadRequest(string firstName)
    {
        // Arrange
        var request = TestEnvironment.ValidRegisterUserRequest with { FirstName = firstName };
        var expectedErrors = new[] { IdentityModuleDomainErrors.UserErrors.InvalidFirstName };

        // Act
        var response = await _httpClient.PostAsJsonAsync(IdentityModuleUrls.User.Register, request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var errors = responseContent.GetErrors();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        ProblemDetailsAssert.HasErrors(expectedErrors, errors);
    }
    
    [Fact]
    public async Task RegisterUser_FirstNameExceedsMaxLength_ReturnBadRequest()
    {
        // Arrange
        var firstNameAsEnumerable = Enumerable.Repeat("a", 101);
        var firstName = string.Join(string.Empty, firstNameAsEnumerable);
        var request = TestEnvironment.ValidRegisterUserRequest with { FirstName = firstName };
        var expectedErrors = new[] { IdentityModuleDomainErrors.UserErrors.NameOutOfRange };

        // Act
        var response = await _httpClient.PostAsJsonAsync(IdentityModuleUrls.User.Register, request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var errors = responseContent.GetErrors();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        ProblemDetailsAssert.HasErrors(expectedErrors, errors);
    }
    
    #endregion

    #region LastName

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    [InlineData("12a")]
    public async Task RegisterUser_InvalidLastName_ReturnBadRequest(string lastName)
    {
        // Arrange
        var request = TestEnvironment.ValidRegisterUserRequest with { LastName = lastName };
        var expectedErrors = new[] { IdentityModuleDomainErrors.UserErrors.InvalidLastName };

        // Act
        var response = await _httpClient.PostAsJsonAsync(IdentityModuleUrls.User.Register, request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var errors = responseContent.GetErrors();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        ProblemDetailsAssert.HasErrors(expectedErrors, errors);
    }
    
    [Fact]
    public async Task RegisterUser_LastNameExceedsMaxLength_ReturnBadRequest()
    {
        // Arrange
        var lastNameAsEnumerable = Enumerable.Repeat("a", 101);
        var lastName = string.Join(string.Empty, lastNameAsEnumerable);
        var request = TestEnvironment.ValidRegisterUserRequest with { LastName = lastName };
        var expectedErrors = new[] { IdentityModuleDomainErrors.UserErrors.NameOutOfRange };

        // Act
        var response = await _httpClient.PostAsJsonAsync(IdentityModuleUrls.User.Register, request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var errors = responseContent.GetErrors();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        ProblemDetailsAssert.HasErrors(expectedErrors, errors);
    }

    #endregion

    #region Email

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    [InlineData("12a")]
    [InlineData("a@b .c")]
    public async Task RegisterUser_InvalidEmail_ReturnBadRequest(string email)
    {
        // Arrange
        var request = TestEnvironment.ValidRegisterUserRequest with { Email = email };
        var expectedErrors = new[] { IdentityModuleDomainErrors.UserErrors.InvalidEmail };

        // Act
        var response = await _httpClient.PostAsJsonAsync(IdentityModuleUrls.User.Register, request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var errors = responseContent.GetErrors();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        ProblemDetailsAssert.HasErrors(expectedErrors, errors);
    }
    
    [Fact]
    public async Task RegisterUser_EmailExceedsMaxLength_ReturnBadRequest()
    {
        // Arrange
        var emailAsEnumerable = Enumerable.Repeat("a", 101);
        var email = string.Join(string.Empty, emailAsEnumerable);
        var request = TestEnvironment.ValidRegisterUserRequest with { Email = email };
        var expectedErrors = new[] { IdentityModuleDomainErrors.UserErrors.EmailOutOfRange };

        // Act
        var response = await _httpClient.PostAsJsonAsync(IdentityModuleUrls.User.Register, request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var errors = responseContent.GetErrors();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        ProblemDetailsAssert.HasErrors(expectedErrors, errors);
    }

    #endregion

    #region Password

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public async Task RegisterUser_InvalidPassword_ReturnBadRequest(string password)
    {
        // Arrange
        var request = TestEnvironment.ValidRegisterUserRequest with { Password = password};
        var expectedErrors = new[] { IdentityModuleDomainErrors.UserErrors.InvalidPassword };

        // Act
        var response = await _httpClient.PostAsJsonAsync(IdentityModuleUrls.User.Register, request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var errors = responseContent.GetErrors();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        ProblemDetailsAssert.HasErrors(expectedErrors, errors);
    }

    #endregion

    #region Roles

    [Fact]
    public async Task RegisterUser_EmptyRoles_ReturnBadRequest()
    {
        // Arrange
        var request = TestEnvironment.ValidRegisterUserRequest with { Roles = [] };
        var expectedErrors = new[] { IdentityModuleDomainErrors.UserErrors.EmptyRoles };

        // Act
        var response = await _httpClient.PostAsJsonAsync(IdentityModuleUrls.User.Register, request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var errors = responseContent.GetErrors();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        ProblemDetailsAssert.HasErrors(expectedErrors, errors);
    }
    
    [Fact]
    public async Task RegisterUser_InvalidRole_ReturnBadRequest()
    {
        // Arrange
        var request = TestEnvironment.ValidRegisterUserRequest with { Roles = [ Role.Doctor.Name, "TestRole" ] };
        var expectedErrors = new[] { IdentityModuleDomainErrors.UserErrors.InvalidRole };

        // Act
        var response = await _httpClient.PostAsJsonAsync(IdentityModuleUrls.User.Register, request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var errors = responseContent.GetErrors();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        ProblemDetailsAssert.HasErrors(expectedErrors, errors);
    }

    #endregion

    #endregion

    private static class TestEnvironment
    {
        internal static RegisterUserRequest ValidRegisterUserRequest => new(FirstName: "John",
            LastName: "Doe",
            Email: "john@doe.com",
            Password: "P@ssw0rd",
            Roles: [Role.Doctor.Name]);
    }
}