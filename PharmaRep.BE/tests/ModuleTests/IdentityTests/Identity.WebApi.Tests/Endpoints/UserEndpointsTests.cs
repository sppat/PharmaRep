using Shared.Tests;

namespace Identity.WebApi.Tests.Endpoints;

[Collection(name: SharedTestConstants.WebApplicationCollectionName)]
public class UserEndpointsTests(WebApplicationFixture fixture) : IClassFixture<WebApplicationFixture>
{
    private readonly HttpClient _httpClient = fixture.CreateClient();
}