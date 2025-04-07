using Shared.Tests;

namespace Identity.WebApi.Tests;

[CollectionDefinition(name: SharedTestConstants.WebApplicationCollectionName)]
public class WebApplicationCollection : ICollectionFixture<WebApplicationFixture>
{

}
