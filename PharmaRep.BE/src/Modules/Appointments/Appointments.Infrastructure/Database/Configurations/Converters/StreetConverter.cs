using Appointments.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace Appointments.Infrastructure.Database.Configurations.Converters;

public class StreetConverter : ValueConverter<AppointmentAddressStreet, string>
{
    public StreetConverter() : base(ToProviderExpression, FromProviderExpression)
    {
    }

    private static readonly Expression<Func<AppointmentAddressStreet, string>> ToProviderExpression = street => ToProvider(street);
    private static readonly Expression<Func<string, AppointmentAddressStreet>> FromProviderExpression = @string => FromProvider(@string);

    private static string ToProvider(AppointmentAddressStreet street) => street.Value;

    private static AppointmentAddressStreet FromProvider(string @string) => @string;
}
