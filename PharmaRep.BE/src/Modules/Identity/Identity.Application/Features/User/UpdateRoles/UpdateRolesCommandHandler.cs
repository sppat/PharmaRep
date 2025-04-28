using Microsoft.AspNetCore.Identity;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.UpdateRoles;

public class UpdateRolesCommandHandler(UserManager<Domain.Entities.User> userManager) : IRequestHandler<UpdateRolesCommand, Result>
{
    public async Task<Result> HandleAsync(UpdateRolesCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}