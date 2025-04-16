using System.Collections.Immutable;

namespace Shared.WebApi;

public record PaginatedResponse<T> where T : class
{
    public readonly int PageNumber;
    public readonly int PageSize;
    public readonly int Total;
    public bool HasNext => PageNumber < Total / PageSize;
    public bool HasPrevious => PageNumber > 1;
    public IReadOnlyCollection<T> Items;

    public PaginatedResponse(int pageNumber, int pageSize, int total, ICollection<T> items)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageNumber, nameof(pageNumber));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize, nameof(pageSize));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(total, nameof(total));
        ArgumentNullException.ThrowIfNull(items);

        PageNumber = pageNumber;
        PageSize = pageSize;
        Total = total;
        Items = items.ToImmutableList();
    }
}