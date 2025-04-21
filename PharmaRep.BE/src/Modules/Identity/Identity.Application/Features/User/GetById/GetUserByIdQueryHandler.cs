using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Identity.Domain.DomainErrors;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.GetById;

public class GetUserByIdQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> HandleAsync(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserAsync(request.UserId, cancellationToken);
        
        return user is null 
            ? Result<UserDto>.Failure([IdentityModuleDomainErrors.UserErrors.UserNotFound], ResultType.NotFound)
            : Result<UserDto>.Success(user);
    }
}