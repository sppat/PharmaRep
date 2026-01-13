using System.Linq.Expressions;

using Appointments.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Appointments.Infrastructure.Database.Configurations.Converters;

public class AppointmentDateConverter : ValueConverter<AppointmentDate, DateTimeOffset>
{
	public AppointmentDateConverter() : base(ToProviderExpression, FromProviderExpression)
	{
	}

	private static readonly Expression<Func<AppointmentDate, DateTimeOffset>> ToProviderExpression = appointmentDate => ToProvider(appointmentDate);
	private static readonly Expression<Func<DateTimeOffset, AppointmentDate>> FromProviderExpression = dateTimeOffset => FromProvider(dateTimeOffset);

	private static DateTimeOffset ToProvider(AppointmentDate appointmentDate) => appointmentDate;
	private static AppointmentDate FromProvider(DateTimeOffset dateTime) => dateTime;
}
