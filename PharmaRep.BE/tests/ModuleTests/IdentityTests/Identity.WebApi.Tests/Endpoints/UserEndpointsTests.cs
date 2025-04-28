using System.Net;
using System.Net.Http.Json;
using Identity.Application.Dtos;
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
    private readonly HttpClient _authorizedHttpClient = fixture.GetAuthorizedClient([Role.Admin.Name]);
    private readonly HttpClient _unauthorizedHttpClient = fixture.GetUnauthorizedClient();
    
    #region Authorization

    [Fact]
    public async Task GetAllUser_UnauthorizedUser_ReturnsForbidden()
    {
        // Arrange

        // Act
        var response = await _unauthorizedHttpClient.GetAsync(IdentityModuleUrls.User.GetAll);
        
        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
    
    [Fact]
    public async Task GetById_UnauthorizedUser_ReturnsForbidden()
    {
        // Arrange
        var getByIdUrl = IdentityModuleUrls.User.GetById.Replace("{id:guid}", Guid.NewGuid().ToString());
        
        // Act
        var response = await _unauthorizedHttpClient.GetAsync(getByIdUrl);
        
        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
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
        var response = await _authorizedHttpClient.GetAsync(url);
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
        var response = await _authorizedHttpClient.GetAsync(url);
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
        var response = await _authorizedHttpClient.GetAsync(uri);
        var responseContent = await response.Content.ReadFromJsonAsync<PaginatedResponse<UserDto>>();
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(pageNumber, responseContent.PageNumber);
        Assert.Equal(pageSize, responseContent.PageSize);
        Assert.True(responseContent.HasNext);
        Assert.True(responseContent.HasPrevious);
    }

    #region PageNumber

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
        var response = await _authorizedHttpClient.GetAsync(uri);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        AssertProblemDetails.HasErrors(expectedErrors, responseContent.GetErrors());
    }

    #endregion

    #region PageSize

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
        var response = await _authorizedHttpClient.GetAsync(uri);
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
        var response = await _authorizedHttpClient.PutAsJsonAsync(updateRolesUrl, request);
        var getUserByIdResponse = await _authorizedHttpClient.GetAsync(getUserByIdUrl);
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
        var response = await _authorizedHttpClient.PutAsJsonAsync(updateRolesUrl, request);
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
        var response = await _authorizedHttpClient.PutAsJsonAsync(updateRolesUrl, request);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        AssertProblemDetails.HasErrors(expectedErrors, responseContent.GetErrors());
    }

    #endregion
}