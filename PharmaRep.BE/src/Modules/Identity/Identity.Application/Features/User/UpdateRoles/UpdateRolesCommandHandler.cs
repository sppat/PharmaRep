using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.UpdateRoles;

public class UpdateRolesCommandHandler : IRequestHandler<UpdateRolesCommand, Result>
{
    public async Task<Result> HandleAsync(UpdateRolesCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}