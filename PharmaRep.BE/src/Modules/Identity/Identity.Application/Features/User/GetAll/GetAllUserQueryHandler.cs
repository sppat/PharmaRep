using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.GetAll;

public class GetAllUserQueryHandler(IUserRepository userRepository) : IRequestHandler<GetAllUsersQuery, Result<IPaginatedResult<UserDto>>>
{
    public async Task<Result<IPaginatedResult<UserDto>>> HandleAsync(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllUsersAsync(pageNumber: request.PageNumber, 
            pageSize: request.PageSize,
            cancellationToken);

        var paginatedResult = new UsersPaginatedResult(PageNumber: request.PageNumber, 
            PageSize: request.PageSize,
            Total: users.Count,
            Items: users);
        
        return Result<IPaginatedResult<UserDto>>.Success(paginatedResult);
    }
}