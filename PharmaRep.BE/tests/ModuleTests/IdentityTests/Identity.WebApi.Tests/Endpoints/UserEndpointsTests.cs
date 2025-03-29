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