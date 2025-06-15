using System.Net.Http.Headers;
using Identity.Infrastructure.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Tests.Auth;
using Shared.Tests.Database;
using Testcontainers.PostgreSql;
using Xunit;

namespace Shared.Tests;

public class WebApplicationFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres:alpine")
        .WithPassword("P@ssw0rd")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var identityDbContext = services.FirstOrDefault(s => s.ServiceType == typeof(PharmaRepIdentityDbContext));
            if (identityDbContext is not null) services.Remove(identityDbContext);

            services.AddDbContext<PharmaRepIdentityDbContext>(options => options.UseSqlServer(_container.GetConnectionString()));
            services.AddAuthentication("TestScheme")
                .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("TestScheme", _ => { });
        });
    }

    public HttpClient GetUnauthorizedClient() => CreateClient();

    public HttpClient GetAuthorizedClient(string[] roles)
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
        client.DefaultRequestHeaders.Add("roles", roles);

        return client;
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        
        using var scope = Services.CreateScope();
        await SeedIdentityModule(scope);
    }

    public new async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    private static async Task SeedIdentityModule(IServiceScope scope)
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<PharmaRepIdentityDbContext>();
        
        await dbContext.Database.ExecuteSqlRawAsync(DatabaseSeeder.IdentityModuleSeeder.SeedTestUsersQuery());
    }
}