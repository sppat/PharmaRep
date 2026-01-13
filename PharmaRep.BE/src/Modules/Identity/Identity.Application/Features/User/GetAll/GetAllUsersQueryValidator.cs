using FluentValidation;

using Shared.Application.Errors;

namespace Identity.Application.Features.User.GetAll;

public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
{
	public GetAllUsersQueryValidator()
	{
		RuleFor(query => query.PageNumber).GreaterThan(0).WithMessage(ApplicationErrors.PaginationErrors.PageNumberOutOfRange);
		RuleFor(query => query.PageSize).GreaterThan(0).WithMessage(ApplicationErrors.PaginationErrors.PageSizeOutOfRange);
	}
}
