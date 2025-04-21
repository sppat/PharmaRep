using Shared.WebApi.Requests;

namespace Identity.WebApi.Requests;

public record GetAllUsersRequest(int PageNumber, int PageSize) : PaginationRequest(PageNumber, PageSize);