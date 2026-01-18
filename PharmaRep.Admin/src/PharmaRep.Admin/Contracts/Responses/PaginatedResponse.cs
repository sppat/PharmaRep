namespace PharmaRep.Admin.Contracts.Responses;

public record PaginatedResponse<T>(int PageNumber, int PageSize, int Total, IReadOnlyCollection<T> Items)
{
	public bool HasNext => PageNumber < Math.Ceiling((double)Total / PageSize);
	public bool HasPrevious => PageNumber > 1;
}
