using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shared.Infrastructure.Converters;

public class NullableUtcConverter() : ValueConverter<DateTimeOffset?, DateTimeOffset?>(
    convertTo => convertTo.HasValue ? convertTo.Value.ToUniversalTime() : convertTo,
    convertFrom => convertFrom) { }