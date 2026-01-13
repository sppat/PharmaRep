using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shared.Infrastructure.Converters;

public class UtcConverter() : ValueConverter<DateTimeOffset, DateTimeOffset>(
	convertTo => convertTo.ToUniversalTime(),
	convertFrom => convertFrom)
{ }
