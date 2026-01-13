using FluentValidation;

using Identity.Domain.DomainErrors;

namespace Identity.Application.Features.User.Delete;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
	public DeleteUserCommandValidator()
	{
		RuleFor(cmd => cmd.UserId).NotEmpty().WithMessage(IdentityModuleDomainErrors.UserErrors.EmptyId);
	}
}
