using System.Linq.Expressions;

using Appointments.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Appointments.Infrastructure.Database.Configurations.Converters;

public class ZipCodeConverter : ValueConverter<AppointmentAddressZipCode, uint>
{
	public ZipCodeConverter() : base(ToProviderExpression, FromProviderExpression)
	{
	}

	private static readonly Expression<Func<AppointmentAddressZipCode, uint>> ToProviderExpression = zipCode => ToProvider(zipCode);
	private static readonly Expression<Func<uint, AppointmentAddressZipCode>> FromProviderExpression = @uint => FromProvider(@uint);

	private static uint ToProvider(AppointmentAddressZipCode zipCode) => zipCode.Value;

	private static AppointmentAddressZipCode FromProvider(uint @uint) => @uint;
}
