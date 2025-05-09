using System.Net;
using System.Net.Http.Json;
using Identity.Application.Dtos;
using Identity.Application.Features.Auth.Register;
using Identity.Domain.DomainErrors;
using Identity.Domain.Entities;
using Identity.WebApi.Endpoints;
using Identity.WebApi.Requests;
using Identity.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Shared.Application.Errors;
using Shared.Tests;
using Shared.Tests.Database;
using Shared.WebApi.Responses;

namespace Identity.WebApi.Tests.Endpoints;

[Collection(name: SharedTestConstants.WebApplicationCollectionName)]
public class UserEndpointsTests(WebApplicationFixture fixture)
{
    private readonly HttpClient _adminHttpClient = fixture.GetAuthorizedClient([Role.Admin.Name]);
    private readonly HttpClient _authorizedHttpClient = fixture.GetAuthorizedClient([Role.Doctor.Name]);
    private readonly HttpClient _unauthorizedHttpClient = fixture.GetUnauthorizedClient();
    
    #region Authorization

    [Theory]
    [InlineData("Doctor")]
    [InlineData("Midwife")]
    public async Task GetAllUser_UnauthorizedUser_ReturnsForbidden(string role)
    {
        // Arrange
        var client = fixture.GetAuthorizedClient([role]);

        // Act
        var response = await client.GetAsync(IdentityModuleUrls.User.GetAll);
        
        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
    
    [Theory]
    [InlineData("Doctor")]
    [InlineData("Midwife")]
    public async Task GetById_UnauthorizedUser_ReturnsForbidden(string role)
    {
        // Arrange
        var client = fixture.GetAuthorizedClient([role]);
        var getByIdUrl = IdentityModuleUrls.User.GetById.Replace("{id:guid}", Guid.NewGuid().ToString());
        
        // Act
        var response = await client.GetAsync(getByIdUrl);
        
        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
    
    [Theory]
    [InlineData("Doctor")]
    [InlineData("Midwife")]
    public async Task UpdateRoles_UnauthorizedUser_ReturnsForbidden(string role)
    {
        // Arrange
        var client = fixture.GetAuthorizedClient([role]);
        var updateRolesUrl = IdentityModuleUrls.User.UpdateRoles.Replace("{id:guid}", Guid.NewGuid().ToString());
        var request = new UpdateRolesRequest([Role.Doctor.Name]);
        
        // Act
        var response = await client.PutAsJsonAsync(updateRolesUrl, request);
        
        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
    
    [Theory]
    [InlineData("Doctor")]
    [InlineData("Midwife")]
    public async Task DeleteById_UnauthorizedUser_ReturnsForbidden(string role)
    {
        // Arrange
        var client = fixture.GetAuthorizedClient([role]);
        var deleteUrl = IdentityModuleUrls.User.Delete.Replace("{id:guid}", Guid.NewGuid().ToString());
        
        // Act
        var response = await client.DeleteAsync(deleteUrl);
        
        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task UpdatePersonalInfo_UnauthorizedUser_ReturnsForbidden()
    {
        // Arrange
        var updatePersonalInfoUrl = IdentityModuleUrls.User.UpdatePersonalInfo.Replace("{id:guid}", Guid.NewGuid().ToString());
        var request = new UpdatePersonalInfoRequest("FirstName", "LastName");
        
        // Act
        var response = await _unauthorizedHttpClient.PutAsJsonAsync(updatePersonalInfoUrl, request);
        
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    #endregion
    
    #region Get By Id
    
    [Fact]
    public async Task GetById_ReturnsSuccess()
    {
        // Arrange
        var expectedUser = MockData.Users.First();
        var url = IdentityModuleUrls.User.GetById.Replace("{id:guid}", expectedUser.Id.ToString());

        // Act
        var response = await _adminHttpClient.GetAsync(url);
        var responseContent = await response.Content.ReadFromJsonAsync<GetUserByIdResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equivalent(expectedUser, responseContent);
    }

    [Fact]
    public async Task GetById_ReturnsNotFoundWithInvalidId()
    {
        // Arrange
        var expectedErrors = new[] {IdentityModuleDomainErrors.UserErrors.UserNotFound};
        var userId = Guid.NewGuid().ToString();
        var url = IdentityModuleUrls.User.GetById.Replace("{id:guid}", userId);
        
        // Act
        var response = await _adminHttpClient.GetAsync(url);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        AssertProblemDetails.HasErrors(expectedErrors, responseContent.GetErrors());
    }

    #endregion

    #region Get All

    [Fact]
    public async Task GetAll_ReturnsListOfUsers()
    {
        // Arrange
        const int pageNumber = 2;
        const int pageSize = 2;
        var queryParams = new Dictionary<string, string>
        {
            { nameof(pageNumber), pageNumber.ToString() },
            { nameof(pageSize), pageSize.ToString() }
        };
        var uri = QueryHelpers.AddQueryString(IdentityModuleUrls.User.GetAll, queryParams);

        // Act
        var response = await _adminHttpClient.GetAsync(uri);
        var responseContent = await response.Content.ReadFromJsonAsync<PaginatedResponse<UserDto>>();
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(pageNumber, responseContent.PageNumber);
        Assert.Equal(pageSize, responseContent.PageSize);
        Assert.True(responseContent.HasNext);
        Assert.True(responseContent.HasPrevious);
    }

    #region Page Number

    [Theory]
    [InlineData(0, 2)]
    [InlineData(-1, 2)]
    public async Task GetAll_NonPositivePageNumber_ReturnsBadRequest(int pageNumber, int pageSize)
    {
        // Arrange
        var expectedErrors = new[] { ApplicationErrors.PaginationErrors.PageNumberOutOfRange };
        var queryParams = new Dictionary<string, string>
        {
            { nameof(pageNumber), pageNumber.ToString() },
            { nameof(pageSize), pageSize.ToString() }
        };
        var uri = QueryHelpers.AddQueryString(IdentityModuleUrls.User.GetAll, queryParams);
        
        // Act
        var response = await _adminHttpClient.GetAsync(uri);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        AssertProblemDetails.HasErrors(expectedErrors, responseContent.GetErrors());
    }

    #endregion

    #region Page Size

    [Theory]
    [InlineData(1, 0)]
    [InlineData(1, -1)]
    public async Task GetAll_NonPositivePageSize_ReturnsBadRequest(int pageNumber, int pageSize)
    {
        // Arrange
        var expectedErrors = new[] { ApplicationErrors.PaginationErrors.PageSizeOutOfRange };
        var queryParams = new Dictionary<string, string>
        {
            { nameof(pageNumber), pageNumber.ToString() },
            { nameof(pageSize), pageSize.ToString() }
        };
        var uri = QueryHelpers.AddQueryString(IdentityModuleUrls.User.GetAll, queryParams);

        // Act
        var response = await _adminHttpClient.GetAsync(uri);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        AssertProblemDetails.HasErrors(expectedErrors, responseContent.GetErrors());
    }

    #endregion

    #endregion

    #region Roles

    [Fact]
    public async Task UpdateRoles_ReturnsSuccess()
    {
        // Arrange
        var user = MockData.Users.First();
        var updateRolesUrl = IdentityModuleUrls.User.UpdateRoles.Replace("{id:guid}", user.Id.ToString());
        var getUserByIdUrl = IdentityModuleUrls.User.GetById.Replace("{id:guid}", user.Id.ToString());
        var expectedRoles = new[] { Role.Doctor.Name };
        var request = new UpdateRolesRequest(expectedRoles);

        // Act
        var response = await _adminHttpClient.PutAsJsonAsync(updateRolesUrl, request);
        var getUserByIdResponse = await _adminHttpClient.GetAsync(getUserByIdUrl);
        var getUserByIdResponseContent = await getUserByIdResponse.Content.ReadFromJsonAsync<GetUserByIdResponse>();

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(expectedRoles, getUserByIdResponseContent.Roles);
    }
    
    [Fact]
    public async Task UpdateRoles_InvalidRolesList_ReturnsBadRequest()
    {
        // Arrange
        var user = MockData.Users.First();
        var updateRolesUrl = IdentityModuleUrls.User.UpdateRoles.Replace("{id:guid}", user.Id.ToString());
        var request = new UpdateRolesRequest(["Dummy Role One", "Dummy Role Two"]);
        var expectedErrors = new[] { IdentityModuleDomainErrors.UserErrors.InvalidRole };
        
        // Act
        var response = await _adminHttpClient.PutAsJsonAsync(updateRolesUrl, request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        AssertProblemDetails.HasErrors(expectedErrors, responseContent.GetErrors());
    }
    
    [Fact]
    public async Task UpdateRoles_EmptyUserId_ReturnsBadRequest()
    {
        // Arrange
        var updateRolesUrl = IdentityModuleUrls.User.UpdateRoles.Replace("{id:guid}", Guid.Empty.ToString());
        var request = new UpdateRolesRequest([Role.Doctor.Name]);
        var expectedErrors = new[] { IdentityModuleDomainErrors.UserErrors.EmptyId };
        
        // Act
        var response = await _adminHttpClient.PutAsJsonAsync(updateRolesUrl, request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        AssertProblemDetails.HasErrors(expectedErrors, responseContent.GetErrors());
    }

    #endregion

    #region Delete User

    [Fact]
    public async Task DeleteById_ValidRequest_ReturnsNoContent()
    {
        // Arrange
        var registerCommand = new RegisterCommand(FirstName: "Delete", LastName: "User", Email: "delete@user.com", Password: "P@ssw0rd");
        var registerResult = await _adminHttpClient.PostAsJsonAsync(IdentityModuleUrls.Authentication.Register, registerCommand);
        var registerResponse = await registerResult.Content.ReadFromJsonAsync<RegisterResponse>();
        var deleteUrl = IdentityModuleUrls.User.Delete.Replace("{id:guid}", registerResponse.UserId.ToString());
        
        // Act
        var result = await _adminHttpClient.DeleteAsync(deleteUrl);
        var getByIdUrl = IdentityModuleUrls.User.GetById.Replace("{id:guid}", registerResponse.UserId.ToString());
        var getByIdResult = await _adminHttpClient.GetAsync(getByIdUrl);
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, getByIdResult.StatusCode);
    }
    
    [Fact]
    public async Task DeleteById_EmptyUserId_ReturnsBadRequest()
    {
        // Arrange
        var userId = Guid.Empty.ToString();
        var url = IdentityModuleUrls.User.Delete.Replace("{id:guid}", userId);
        var expectedErrors = new[] { IdentityModuleDomainErrors.UserErrors.EmptyId };
        
        // Act
        var result = await _adminHttpClient.DeleteAsync(url);
        var responseContent = await result.Content.ReadFromJsonAsync<ProblemDetails>();
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        AssertProblemDetails.HasErrors(expectedErrors, responseContent.GetErrors());
    }
    
    [Fact]
    public async Task DeleteById_UserNotExist_ReturnsNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var url = IdentityModuleUrls.User.Delete.Replace("{id:guid}", userId);
        var expectedErrors = new[] { IdentityModuleDomainErrors.UserErrors.UserNotFound };
        
        // Act
        var result = await _adminHttpClient.DeleteAsync(url);
        var responseContent = await result.Content.ReadFromJsonAsync<ProblemDetails>();
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        AssertProblemDetails.HasErrors(expectedErrors, responseContent.GetErrors());
    }

    #endregion

    #region Update Personal Info

    [Fact]
    public async Task UpdatePersonalInfo_ValidRequest_ReturnsNoContentAndUpdatedUserInfo()
    {
        // Arrange
        var registerRequest = new RegisterRequest(FirstName: "Update", LastName: "User", Email: "update_one@user.com", Password: "P@ssw0rd");
        var registerResult = await _adminHttpClient.PostAsJsonAsync(IdentityModuleUrls.Authentication.Register, registerRequest);
        var registerResponse = await registerResult.Content.ReadFromJsonAsync<RegisterResponse>();
        var getByIdUrl = IdentityModuleUrls.User.GetById.Replace("{id:guid}", registerResponse.UserId.ToString());
        var updatePersonalInfoUrl = IdentityModuleUrls.User.UpdatePersonalInfo.Replace("{id:guid}", registerResponse.UserId.ToString());
        var updatePersonalInfoRequest = new UpdatePersonalInfoRequest("UpdatedFirstName", "UpdatedLastName");
        
        // Act
        var updatePersonalInfoResult = await _authorizedHttpClient.PutAsJsonAsync(updatePersonalInfoUrl, updatePersonalInfoRequest);
        var getByIdResult = await _adminHttpClient.GetAsync(getByIdUrl);
        var userResponse = await getByIdResult.Content.ReadFromJsonAsync<GetUserByIdResponse>();
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, updatePersonalInfoResult.StatusCode);
        Assert.Equal(updatePersonalInfoRequest.FirstName, userResponse.FirstName);
        Assert.Equal(updatePersonalInfoRequest.LastName, userResponse.LastName);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("1Test")]
    [InlineData("_Hello_")]
    public async Task UpdatePersonalInfo_InvalidFirstName_ReturnsBadRequest(string firstName)
    {
        // Arrange
        var registerRequest = new RegisterRequest(FirstName: "Update", LastName: "User", Email: "update_two@user.com", Password: "P@ssw0rd");
        var registerResult = await _adminHttpClient.PostAsJsonAsync(IdentityModuleUrls.Authentication.Register, registerRequest);
        var registerResponse = await registerResult.Content.ReadFromJsonAsync<RegisterResponse>();
        var updatePersonalInfoUrl = IdentityModuleUrls.User.UpdatePersonalInfo.Replace("{id:guid}", registerResponse.UserId.ToString());
        var updatePersonalInfoRequest = new UpdatePersonalInfoRequest(firstName, "UpdatedLastName");
        var expectedErrors = new[] { IdentityModuleDomainErrors.UserErrors.InvalidFirstName };
        
        // Act
        var updatePersonalInfoResult = await _authorizedHttpClient.PutAsJsonAsync(updatePersonalInfoUrl, updatePersonalInfoRequest);
        var updatePersonalInfoResponse = await updatePersonalInfoResult.Content.ReadFromJsonAsync<ProblemDetails>();
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, updatePersonalInfoResult.StatusCode);
        AssertProblemDetails.HasErrors(expectedErrors, updatePersonalInfoResponse.GetErrors());
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("1Test")]
    [InlineData("_Hello_")]
    public async Task UpdatePersonalInfo_InvalidLastName_ReturnsBadRequest(string lastName)
    {
        // Arrange
        var registerRequest = new RegisterRequest(FirstName: "Update", LastName: "User", Email: "update_three@user.com", Password: "P@ssw0rd");
        var registerResult = await _adminHttpClient.PostAsJsonAsync(IdentityModuleUrls.Authentication.Register, registerRequest);
        var registerResponse = await registerResult.Content.ReadFromJsonAsync<RegisterResponse>();
        var updatePersonalInfoUrl = IdentityModuleUrls.User.UpdatePersonalInfo.Replace("{id:guid}", registerResponse.UserId.ToString());
        var updatePersonalInfoRequest = new UpdatePersonalInfoRequest("UpdatedFirstName", lastName);
        var expectedErrors = new[] { IdentityModuleDomainErrors.UserErrors.InvalidLastName };
        
        // Act
        var updatePersonalInfoResult = await _authorizedHttpClient.PutAsJsonAsync(updatePersonalInfoUrl, updatePersonalInfoRequest);
        var updatePersonalInfoResponse = await updatePersonalInfoResult.Content.ReadFromJsonAsync<ProblemDetails>();
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, updatePersonalInfoResult.StatusCode);
        AssertProblemDetails.HasErrors(expectedErrors, updatePersonalInfoResponse.GetErrors());
    }

    #endregion
}