using Identity.Domain.DomainErrors;

using Microsoft.AspNetCore.Identity;

using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.UpdateRoles;

public class UpdateRolesCommandHandler(UserManager<Domain.Entities.User> userManager) : IRequestHandler<UpdateRolesCommand, Result<Unit>>
{
	public async Task<Result<Unit>> HandleAsync(UpdateRolesCommand request, CancellationToken cancellationToken)
	{
		var user = await userManager.FindByIdAsync(request.UserId.ToString());
		if (user is null)
		{
			return Result<Unit>.Failure([IdentityModuleDomainErrors.UserErrors.UserNotFound], ResultType.NotFound);
		}

		var existingRoles = await userManager.GetRolesAsync(user);
		var rolesToRemove = existingRoles.Except(request.Roles);
		var removeRolesResult = await userManager.RemoveFromRolesAsync(user, rolesToRemove);
		if (!removeRolesResult.Succeeded)
		{
			var removeRolesErrors = removeRolesResult.Errors.Select(e => e.Description).ToArray();
			return Result<Unit>.Failure(removeRolesErrors, ResultType.ServerError);
		}

		var rolesToAdd = request.Roles.Except(existingRoles);
		var addRolesResult = await userManager.AddToRolesAsync(user, rolesToAdd);
		if (addRolesResult.Succeeded)
		{
			return Result<Unit>.Success(Unit.Value, ResultType.Updated);
		}

		var addRolesErrors = addRolesResult.Errors.Select(e => e.Description).ToArray();
		return Result<Unit>.Failure(addRolesErrors, ResultType.ServerError);
	}
}
