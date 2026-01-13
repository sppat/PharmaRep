using Identity.Application.Dtos;

using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.GetMe;

public record GetMeQuery(Guid UserId) : IRequest<Result<MeDto>>;
