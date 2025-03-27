using Identity.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.MsSql;
using Xunit;

namespace Shared.Tests;

public class WebApplicationFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithPassword("P@ssw0rd")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Staging);
        builder.ConfigureTestServices(services =>
        {
            var identityDbContext = services.FirstOrDefault(s => s.ServiceType == typeof(PharmaRepIdentityDbContext));
            if (identityDbContext is not null) services.Remove(identityDbContext);

            services.AddDbContext<PharmaRepIdentityDbContext>(options => options.UseSqlServer(_container.GetConnectionString()));
        });
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        var connectionString = _container.GetConnectionString();
    }

    public new async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}