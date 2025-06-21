using Identity.Public.Abstractions;
using Identity.Public.Contracts;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Public.Features.GetUsersBasicInfo;

public class GetUsersBasicInfoQueryHandler(IPublicUserRepository publicUserRepository) : IRequestHandler<GetUsersBasicInfoQuery, Result<IEnumerable<UserBasicInfo>>>
{
    public async Task<Result<IEnumerable<UserBasicInfo>>> HandleAsync(GetUsersBasicInfoQuery request, CancellationToken cancellationToken)
    {
        var usersBasicInfo = await publicUserRepository.GetUsersBasicInfoAsync(request.UsersId, cancellationToken);
        
        return Result<IEnumerable<UserBasicInfo>>.Success(usersBasicInfo);
    }
}