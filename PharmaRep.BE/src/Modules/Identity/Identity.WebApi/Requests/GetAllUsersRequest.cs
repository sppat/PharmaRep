namespace Identity.WebApi.Requests;

public record GetAllUsersRequest(int PageNumber = 1, int PageSize = 10);
