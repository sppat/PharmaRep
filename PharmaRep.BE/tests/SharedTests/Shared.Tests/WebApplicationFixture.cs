using DotNet.Testcontainers.Containers;
using Identity.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddDbContext<PharmaRepIdentityDbContext>(options => options.UseNpgsql(_container.GetConnectionString()));
        });
    }

    public async Task<ExecResult> ExecuteQueryAsync(string query)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(query);
        
        return await _container.ExecScriptAsync(query);
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}