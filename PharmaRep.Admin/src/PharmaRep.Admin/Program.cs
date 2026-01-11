using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using PharmaRep.Admin;
using PharmaRep.Admin.Configurations;
using PharmaRep.Admin.Services;
using PharmaRep.Admin.Utils;
using PharmaRep.Admin.Utils.Clients;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.Configure<ApiClientConfiguration>(builder.Configuration.GetSection(ApiClientConfiguration.Section));
builder.Services.AddScoped<BearerTokenHandler>();
builder.Services.AddScoped(serviceProvider =>
{
    var apiClientConfiguration = serviceProvider.GetRequiredService<IOptions<ApiClientConfiguration>>()?.Value;
    if (string.IsNullOrWhiteSpace(apiClientConfiguration?.BaseAddress))
    {
        throw new ArgumentNullException(nameof(ApiClientConfiguration.BaseAddress));
    }

    var bearerHandler = serviceProvider.GetRequiredService<BearerTokenHandler>();
    bearerHandler.InnerHandler = new HttpClientHandler();

	return new HttpClient(bearerHandler) { BaseAddress = new Uri(apiClientConfiguration.BaseAddress) };
});

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<UserApiClient>();
builder.Services.AddScoped<AuthApiClient>();

builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

await builder.Build().RunAsync();
