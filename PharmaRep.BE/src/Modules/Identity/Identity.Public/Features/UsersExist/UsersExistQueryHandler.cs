using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Public.Features.UsersExist;

public class UsersExistQueryHandler(UserManager<User> userManager) : IRequestHandler<UsersExistQuery, Result<Unit>>
{
    public Task<Result<Unit>> HandleAsync(UsersExistQuery request, CancellationToken cancellationToken)
    {
        foreach (var userId in request.UserIds)
        {
            var userExist = userManager.Users.Any(user => user.Id == userId);
            if (userExist) continue;

            return Task.FromResult(Result<Unit>.Failure([$"User with id {userId} does not exist"], ResultType.NotFound));
        }

        return Task.FromResult(Result<Unit>.Success(Unit.Value));
    }
}