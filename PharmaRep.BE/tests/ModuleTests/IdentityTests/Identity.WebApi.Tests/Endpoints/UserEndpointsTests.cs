using System.Net;
using System.Net.Http.Json;
using System.Text;
using Identity.Application.Dtos;
using Identity.Domain.DomainErrors;
using Identity.Domain.Entities;
using Identity.WebApi.Endpoints;
using Identity.WebApi.Requests;
using Identity.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Shared.Tests;
using Shared.WebApi.Responses;
using Xunit.Abstractions;

namespace Identity.WebApi.Tests.Endpoints;

[Collection(name: SharedTestConstants.WebApplicationCollectionName)]
public class UserEndpointsTests(WebApplicationFixture fixture, ITestOutputHelper outputHelper)
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
        AssertProblemDetails.HasErrors(expectedErrors, errors);
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
        AssertProblemDetails.HasErrors(expectedErrors, errors);
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
        AssertProblemDetails.HasErrors(expectedErrors, errors);
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
        AssertProblemDetails.HasErrors(expectedErrors, errors);
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
        AssertProblemDetails.HasErrors(expectedErrors, errors);
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
        AssertProblemDetails.HasErrors(expectedErrors, errors);
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
        var request = TestEnvironment.ValidRegisterUserRequest with { Password = password };
        var expectedErrors = new[] { IdentityModuleDomainErrors.UserErrors.InvalidPassword };

        // Act
        var response = await _httpClient.PostAsJsonAsync(IdentityModuleUrls.User.Register, request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var errors = responseContent.GetErrors();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        AssertProblemDetails.HasErrors(expectedErrors, errors);
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
        AssertProblemDetails.HasErrors(expectedErrors, errors);
    }

    [Fact]
    public async Task RegisterUser_InvalidRole_ReturnBadRequest()
    {
        // Arrange
        var request = TestEnvironment.ValidRegisterUserRequest with { Roles = [Role.Doctor.Name, "TestRole"] };
        var expectedErrors = new[] { IdentityModuleDomainErrors.UserErrors.InvalidRole };

        // Act
        var response = await _httpClient.PostAsJsonAsync(IdentityModuleUrls.User.Register, request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var errors = responseContent.GetErrors();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        AssertProblemDetails.HasErrors(expectedErrors, errors);
    }

    #endregion

    #endregion

    #region Get By Id
    
    [Fact]
    public async Task GetById_ReturnsSuccess()
    {
        // Arrange
        var queryResult = await fixture.ExecuteQueryAsync(TestEnvironment.SeedTestUserQuery());
        if (queryResult.ExitCode is not 0)
        {
            outputHelper.WriteLine(queryResult.Stderr);
        }
        
        var expectedUserId = TestEnvironment.ExpectedGetUserByIdResponse.Id.ToString();
        var url = IdentityModuleUrls.User.GetById.Replace("{id:guid}", expectedUserId);

        // Act
        var response = await _httpClient.GetAsync(url);
        var responseContent = await response.Content.ReadFromJsonAsync<GetUserByIdResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equivalent(TestEnvironment.ExpectedGetUserByIdResponse, responseContent);
    }

    [Fact]
    public async Task GetById_ReturnsNotFoundWithInvalidId()
    {
        // Arrange
        var expectedErrors = new[] {IdentityModuleDomainErrors.UserErrors.UserNotFound};
        var userId = Guid.NewGuid().ToString();
        var url = IdentityModuleUrls.User.GetById.Replace("{id:guid}", userId);
        
        // Act
        var response = await _httpClient.GetAsync(url);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        AssertProblemDetails.HasErrors(expectedErrors, responseContent.GetErrors());
    }

    #endregion

    #region GetAll

    [Fact]
    public async Task GetAll_ReturnsListOfUsers()
    {
        // Arrange
        var queryResult = await fixture.ExecuteQueryAsync(TestEnvironment.SeedGetAllTestUsersQuery());
        if (queryResult.ExitCode is not 0)
        {
            outputHelper.WriteLine(queryResult.Stderr);
        }
        const int pageNumber = 2;
        const int pageSize = 2;
        var queryParams = new Dictionary<string, string>
        {
            { nameof(pageNumber), pageNumber.ToString() },
            { nameof(pageSize), pageSize.ToString() }
        };
        var uri = QueryHelpers.AddQueryString(IdentityModuleUrls.User.GetAll, queryParams);
        
        // Act
        var response = await _httpClient.GetAsync(uri);
        var responseContent = await response.Content.ReadFromJsonAsync<PaginatedResponse<UserDto>>();
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(pageNumber, responseContent.PageNumber);
        Assert.Equal(pageSize, responseContent.PageSize);
        Assert.True(responseContent.HasNext);
        Assert.True(responseContent.HasPrevious);
        Assert.Equivalent(TestEnvironment.GetAllUsersTestData, responseContent.Items);
    }

    #endregion
    
    private static class TestEnvironment
    {
        internal static RegisterUserRequest ValidRegisterUserRequest => new(FirstName: "John",
            LastName: "Doe",
            Email: "john@doe.com",
            Password: "P@ssw0rd",
            Roles: [Role.Doctor.Name]);

        internal static GetUserByIdResponse ExpectedGetUserByIdResponse => new(Id: Guid.Parse("8f92f601-658f-4f9a-8a61-afe2f9f77ed1"),
            FirstName: "Test",
            LastName: "User",
            Email: "test@user.com",
            Roles: [Role.Doctor.Name]);

        internal static IEnumerable<UserDto> GetAllUsersTestData => 
        [
            new(Id: Guid.Parse("23b42b97-972b-40e9-9f43-baa9b2d3d1ad"),
                FirstName: "User",
                LastName: "One",
                Email: "user@one.com",
                Roles: [Role.MedicalRepresentative.Name]),
            new(Id: Guid.Parse("525eed83-52ec-44f7-8f2b-0c42e9e8e112"),
                FirstName: "User",
                LastName: "Two",
                Email: "user@two.com",
                Roles: [Role.MedicalRepresentative.Name]),
            new(Id: Guid.Parse("18309ae7-c39e-4c9b-9cb3-deb592eb6980"),
                FirstName: "User",
                LastName: "Three",
                Email: "user@three.com",
                Roles: [Role.MedicalRepresentative.Name]),
            new(Id: Guid.Parse("a08c2300-6a09-46ea-83bf-c75b4ab6485d"),
                FirstName: "User",
                LastName: "Four",
                Email: "user@four.com",
                Roles: [Role.MedicalRepresentative.Name]),
            new(Id: Guid.Parse("3c1a09b3-1b46-4b3c-8c10-aec8296239b5"),
                FirstName: "User",
                LastName: "Five",
                Email: "user@five.com",
                Roles: [Role.MedicalRepresentative.Name])
        ];

        internal static string SeedTestUserQuery() => $"""
                                                       INSERT INTO identity."AspNetUsers" ("Id", "FirstName", "LastName", "Email", "EmailConfirmed", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount")
                                                       VALUES ('{ExpectedGetUserByIdResponse.Id}', '{ExpectedGetUserByIdResponse.FirstName}', '{ExpectedGetUserByIdResponse.LastName}', '{ExpectedGetUserByIdResponse.Email}', true, false, false, false, 0);
                                                       
                                                       INSERT INTO identity."AspNetUserRoles" ("UserId", "RoleId")
                                                       VALUES ('{ExpectedGetUserByIdResponse.Id}', '{Role.Doctor.Id}');
                                                       """;

        internal static string SeedGetAllTestUsersQuery()
        {
            var queryBuilder = new StringBuilder();
            foreach (var user in GetAllUsersTestData)
            {
                var addUserQuery = $"""
                                    INSERT INTO identity."AspNetUsers" ("Id", "FirstName", "LastName", "Email", "EmailConfirmed", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnabled", "AccessFailedCount")
                                    VALUES ('{user.Id}', '{user.FirstName}', '{user.LastName}', '{user.Email}', true, false, false, false, 0);
                                    INSERT INTO identity."AspNetUserRoles" ("UserId", "RoleId")
                                    VALUES ('{user.Id}', '{Role.MedicalRepresentative.Id}');
                                    """;
                queryBuilder.Append(addUserQuery);
                queryBuilder.AppendLine();
            }
            
            return queryBuilder.ToString();
        }
    }
}