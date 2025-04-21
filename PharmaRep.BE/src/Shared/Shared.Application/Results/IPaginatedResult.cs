namespace Shared.Application.Results;

public interface IPaginatedResult<out T>
{
    int PageNumber { get; }
    int PageSize { get; }
    int Total { get; }
    IEnumerable<T> Items { get; }
}