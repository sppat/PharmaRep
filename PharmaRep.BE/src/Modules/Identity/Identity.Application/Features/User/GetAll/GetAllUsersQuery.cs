using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.GetAll;

public record GetAllUsersQuery(int PageNumber, int PageSize) : IRequest<Result<UsersPaginatedResult>>;