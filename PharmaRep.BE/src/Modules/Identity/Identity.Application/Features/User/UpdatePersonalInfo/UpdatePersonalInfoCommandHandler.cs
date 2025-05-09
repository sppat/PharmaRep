using Identity.Application.Interfaces;
using Identity.Domain.DomainErrors;
using Microsoft.AspNetCore.Identity;
using Shared.Application;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.UpdatePersonalInfo;

public class UpdatePersonalInfoCommandHandler(IUserRepository userRepository,
    IIdentityUnitOfWork unitOfWork) : IRequestHandler<UpdatePersonalInfoCommand, Result>
{
    public async Task<Result> HandleAsync(UpdatePersonalInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(errors: [IdentityModuleDomainErrors.UserErrors.UserNotFound], ResultType.NotFound);
        }

        if (string.Equals(user.FirstName, request.FirstName) && string.Equals(user.LastName, request.LastName))
        {
            return Result.Success(ResultType.Updated);
        }
        
        user.UpdateFirstName(request.FirstName);
        user.UpdateLastName(request.LastName);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success(ResultType.Updated);
    }
}