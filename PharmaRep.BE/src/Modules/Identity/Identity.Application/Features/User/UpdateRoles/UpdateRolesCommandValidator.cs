using FluentValidation;

using Identity.Domain.DomainErrors;
using Identity.Domain.Entities;

namespace Identity.Application.Features.User.UpdateRoles;

public class UpdateRolesCommandValidator : AbstractValidator<UpdateRolesCommand>
{
	public UpdateRolesCommandValidator()
	{
		RuleFor(cmd => cmd.UserId)
			.NotEmpty()
			.WithMessage(IdentityModuleDomainErrors.UserErrors.EmptyId);

		RuleFor(cmd => cmd.Roles)
			.Must(RolesExist)
			.When(cmd => cmd.Roles is not null)
			.WithMessage(IdentityModuleDomainErrors.UserErrors.InvalidRole);
	}

	private static bool RolesExist(IEnumerable<string> roles)
	{
		var validRoles = Role.All.Select(r => r.Name).ToList();

		return roles.All(r => validRoles.Contains(r, StringComparer.InvariantCultureIgnoreCase));
	}
}
