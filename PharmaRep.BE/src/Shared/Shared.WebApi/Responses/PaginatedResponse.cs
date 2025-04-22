namespace Shared.WebApi.Responses;

public record PaginatedResponse<T>
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public int Total { get; }
    public bool HasNext => PageNumber < Math.Ceiling((double)Total / PageSize);
    public bool HasPrevious => PageNumber > 1;
    public IReadOnlyCollection<T> Items { get; }

public PaginatedResponse(int pageNumber, int pageSize, int total, IReadOnlyCollection<T> items)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageNumber, nameof(pageNumber));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize, nameof(pageSize));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(total, nameof(total));
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        PageNumber = pageNumber;
        PageSize = pageSize;
        Total = total;
        Items = items;
    }
}