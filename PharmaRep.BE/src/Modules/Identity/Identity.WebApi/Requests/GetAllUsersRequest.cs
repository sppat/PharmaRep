using Shared.WebApi.Requests;

namespace Identity.WebApi.Requests;

public record GetAllUsersRequest(int PageNumber = 1, int PageSize = 10) : PaginationRequest(PageNumber, PageSize);