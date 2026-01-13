using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.UpdatePersonalInfo;

public record UpdatePersonalInfoCommand(Guid UserId, string FirstName, string LastName) : IRequest<Result<Unit>>;
