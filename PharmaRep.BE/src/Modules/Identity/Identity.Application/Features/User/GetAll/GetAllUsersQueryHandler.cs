using Identity.Application.Interfaces;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.GetAll;

public class GetAllUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetAllUsersQuery, Result<UsersPaginatedResult>>
{
    public async Task<Result<UsersPaginatedResult>> HandleAsync(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var totalUsers = await userRepository.CountAsync(cancellationToken);
        var users = await userRepository.GetAllUsersAsync(pageNumber: request.PageNumber, 
            pageSize: request.PageSize,
            cancellationToken);

        var paginatedResult = new UsersPaginatedResult(PageNumber: request.PageNumber, 
            PageSize: request.PageSize,
            Total: totalUsers,
            Items: users);
        
        return Result<UsersPaginatedResult>.Success(paginatedResult);
    }
}