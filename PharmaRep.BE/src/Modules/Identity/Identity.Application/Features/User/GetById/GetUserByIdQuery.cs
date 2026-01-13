using Identity.Application.Dtos;

using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.GetById;

public record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserDto>>;
