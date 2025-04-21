using System.Collections.Immutable;

namespace Shared.WebApi.Responses;

public record PaginatedResponse<T>
{
    public readonly int PageNumber;
    public readonly int PageSize;
    public readonly int Total;
    public bool HasNext => PageNumber < Math.Ceiling((double)Total / PageSize);
    public bool HasPrevious => PageNumber > 1;
    public IReadOnlyCollection<T> Items;

    public PaginatedResponse(int pageNumber, int pageSize, int total, ICollection<T> items)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageNumber, nameof(pageNumber));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize, nameof(pageSize));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(total, nameof(total));
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        PageNumber = pageNumber;
        PageSize = pageSize;
        Total = total;
        Items = items.ToImmutableList();
    }
}