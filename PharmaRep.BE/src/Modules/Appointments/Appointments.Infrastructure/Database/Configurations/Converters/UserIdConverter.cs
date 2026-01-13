using System.Linq.Expressions;

using Appointments.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Appointments.Infrastructure.Database.Configurations.Converters;

public class UserIdConverter : ValueConverter<UserId, Guid?>
{
	public UserIdConverter() : base(ToProviderExpression, FromProviderExpression)
	{

	}

	private static readonly Expression<Func<UserId, Guid?>> ToProviderExpression = userId => ToProvider(userId);
	private static readonly Expression<Func<Guid?, UserId>> FromProviderExpression = guid => FromProvider(guid);

	private static Guid? ToProvider(UserId userId) => userId?.Value;

	private static UserId FromProvider(Guid? guid) => guid;
}
