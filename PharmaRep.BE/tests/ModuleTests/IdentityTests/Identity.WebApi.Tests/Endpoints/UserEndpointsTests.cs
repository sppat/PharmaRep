using Shared.Tests;

namespace Identity.WebApi.Tests.Endpoints;

public class UserEndpointsTests(WebApplicationFixture fixture) : IClassFixture<WebApplicationFixture>
{
    private readonly HttpClient _httpClient = fixture.CreateClient();
}