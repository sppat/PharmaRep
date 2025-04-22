using DotNet.Testcontainers.Containers;
using Identity.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddDbContext<PharmaRepIdentityDbContext>(options => options.UseNpgsql(_container.GetConnectionString()));
        });
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