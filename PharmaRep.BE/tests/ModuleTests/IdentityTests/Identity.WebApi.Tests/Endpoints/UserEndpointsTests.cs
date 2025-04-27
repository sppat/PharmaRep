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
    private readonly HttpClient _httpClient = fixture.CreateClient();

    #region Get By Id
    
    [Fact]
    public async Task GetById_ReturnsSuccess()
    {
        // Arrange
        var expectedUser = MockData.Users.First();
        var url = IdentityModuleUrls.User.GetById.Replace("{id:guid}", expectedUser.Id.ToString());

        // Act
        var response = await _httpClient.GetAsync(url);
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
        const int pageNumber = 2;
        const int pageSize = 2;
        var expectedUsers = MockData.Users.OrderBy(u => u.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
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
        Assert.Equivalent(expectedUsers, responseContent.Items);
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
        var response = await _httpClient.GetAsync(uri);
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
        var response = await _httpClient.GetAsync(uri);
        var responseContent = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        AssertProblemDetails.HasErrors(expectedErrors, responseContent.GetErrors());
    }

    #endregion

    #endregion

    
}