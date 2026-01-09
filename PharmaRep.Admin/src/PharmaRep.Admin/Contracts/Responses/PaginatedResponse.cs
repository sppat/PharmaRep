namespace PharmaRep.Admin.Contracts.Responses;

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
		PageNumber = pageNumber;
		PageSize = pageSize;
		Total = total;
		Items = items;
	}
}
