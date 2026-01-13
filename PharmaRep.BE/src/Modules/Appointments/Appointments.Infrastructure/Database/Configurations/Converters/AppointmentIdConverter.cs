using System.Linq.Expressions;

using Appointments.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Appointments.Infrastructure.Database.Configurations.Converters;

public class AppointmentIdConverter : ValueConverter<AppointmentId, Guid>
{
	public AppointmentIdConverter() : base(ToProviderExpression, FromProviderExpression)
	{

	}

	private static readonly Expression<Func<AppointmentId, Guid>> ToProviderExpression = appointmentId => ToProvider(appointmentId);
	private static readonly Expression<Func<Guid, AppointmentId>> FromProviderExpression = guid => FromProvider(guid);

	private static Guid ToProvider(AppointmentId appointmentId) => appointmentId.Value;

	private static AppointmentId FromProvider(Guid guid) => guid;
}
