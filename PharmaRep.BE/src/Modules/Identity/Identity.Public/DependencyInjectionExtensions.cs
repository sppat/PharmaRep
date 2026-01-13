using Microsoft.Extensions.DependencyInjection;

namespace Identity.Public;

public static class DependencyInjectionExtensions
{
	public static IServiceCollection AddPublicIdentityModule(this IServiceCollection services)
	{
		return services;
	}
}
