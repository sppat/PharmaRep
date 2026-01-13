using FluentValidation;

using Identity.Domain.DomainErrors;
using Identity.Domain.RegexConstants;

namespace Identity.Application.Features.User.UpdatePersonalInfo;

public class UpdatePersonalInfoCommandValidator : AbstractValidator<UpdatePersonalInfoCommand>
{
	private const int MaxNameLength = 50;

	public UpdatePersonalInfoCommandValidator()
	{
		#region Id

		RuleFor(req => req.UserId)
			.NotEmpty()
			.WithMessage(IdentityModuleDomainErrors.UserErrors.EmptyId);

		#endregion

		#region FirstName

		RuleFor(req => req.FirstName)
			.NotNull()
			.WithMessage(IdentityModuleDomainErrors.UserErrors.InvalidFirstName);

		When(req => req.FirstName is not null, () =>
		{
			RuleFor(req => req.FirstName)
				.MaximumLength(MaxNameLength)
				.WithMessage(IdentityModuleDomainErrors.UserErrors.NameOutOfRange);

			RuleFor(req => req.FirstName)
				.Must(UserRegex.NameFormat().IsMatch)
				.WithMessage(IdentityModuleDomainErrors.UserErrors.InvalidFirstName);
		});

		#endregion

		#region LastName

		RuleFor(req => req.LastName)
			.NotNull()
			.WithMessage(IdentityModuleDomainErrors.UserErrors.InvalidLastName);

		When(req => req.LastName is not null, () =>
		{
			RuleFor(req => req.LastName)
				.MaximumLength(MaxNameLength)
				.WithMessage(IdentityModuleDomainErrors.UserErrors.NameOutOfRange);

			RuleFor(req => req.LastName)
				.Must(UserRegex.NameFormat().IsMatch)
				.WithMessage(IdentityModuleDomainErrors.UserErrors.InvalidLastName);
		});

		#endregion
	}
}
