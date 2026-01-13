using Identity.Domain.Entities;

using Microsoft.AspNetCore.Authorization;

namespace Identity.Infrastructure;

public static class AuthPolicy
{
	public static class AdminPolicy
	{
		public static string Name => Role.Admin.Name;

		public static void Configure(AuthorizationPolicyBuilder builder)
		{
			builder.RequireRole(Role.Admin.Name!);
		}
	}
}
