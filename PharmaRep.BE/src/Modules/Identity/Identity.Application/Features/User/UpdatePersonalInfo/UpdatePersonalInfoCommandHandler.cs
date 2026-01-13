using Identity.Application.Interfaces;
using Identity.Domain.DomainErrors;

using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.UpdatePersonalInfo;

public class UpdatePersonalInfoCommandHandler(IUserRepository userRepository,
	IIdentityUnitOfWork unitOfWork) : IRequestHandler<UpdatePersonalInfoCommand, Result<Unit>>
{
	public async Task<Result<Unit>> HandleAsync(UpdatePersonalInfoCommand request, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetUserAsync(request.UserId, cancellationToken);
		if (user is null)
		{
			return Result<Unit>.Failure(errors: [IdentityModuleDomainErrors.UserErrors.UserNotFound], ResultType.NotFound);
		}

		if (string.Equals(user.FirstName, request.FirstName) && string.Equals(user.LastName, request.LastName))
		{
			return Result<Unit>.Success(Unit.Value, ResultType.Updated);
		}

		user.UpdateFirstName(request.FirstName);
		user.UpdateLastName(request.LastName);
		await unitOfWork.SaveChangesAsync(cancellationToken);

		return Result<Unit>.Success(Unit.Value, ResultType.Updated);
	}
}
