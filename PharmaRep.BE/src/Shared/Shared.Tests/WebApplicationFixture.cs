using System.Net.Http.Headers;

using Appointments.Infrastructure.Database;

using Identity.Infrastructure.Database;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Shared.Tests.Auth;
using Shared.Tests.Database;

using Testcontainers.MsSql;

using Xunit;

namespace Shared.Tests;

public class WebApplicationFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
	private readonly MsSqlContainer _container = new MsSqlBuilder()
		.Build();

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureTestServices(services =>
		{
			var identityDbContext = services.FirstOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == typeof(PharmaRepIdentityDbContext));
			if (identityDbContext is not null)
			{
				services.Remove(identityDbContext);
			}

			var appointmentDbContext = services.FirstOrDefault(serviceDescriptor => serviceDescriptor.ServiceType == typeof(PharmaRepAppointmentsDbContext));
			if (appointmentDbContext is not null)
			{
				services.Remove(appointmentDbContext);
			}

			services.AddDbContext<PharmaRepIdentityDbContext>(options => options.UseSqlServer(_container.GetConnectionString()));
			services.AddDbContext<PharmaRepAppointmentsDbContext>(options => options.UseSqlServer(_container.GetConnectionString()));

			services.AddAuthentication("TestScheme")
				.AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("TestScheme", _ => { });
		});
	}

	public HttpClient GetUnauthorizedClient() => CreateClient();

	public HttpClient GetAuthorizedClient(string[] roles, string id = null)
	{
		var client = CreateClient();
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
		client.DefaultRequestHeaders.Add("roles", roles);

		if (id is not null)
		{
			client.DefaultRequestHeaders.Add("id", id);
		}

		return client;
	}

	public async Task InitializeAsync()
	{
		await _container.StartAsync();

		using var scope = Services.CreateScope();
		await SeedModule<PharmaRepIdentityDbContext>(scope, DatabaseSeeder.IdentityModuleSeeder.SeedTestUsersQuery());
		await SeedModule<PharmaRepAppointmentsDbContext>(scope, DatabaseSeeder.AppointmentsModuleSeeder.SeedAppointment());
	}

	public new async Task DisposeAsync()
	{
		await _container.DisposeAsync();
	}

	private static async Task SeedModule<TContext>(IServiceScope scope, string query) where TContext : DbContext
	{
		var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

		await dbContext.Database.ExecuteSqlRawAsync(query);
	}
}
