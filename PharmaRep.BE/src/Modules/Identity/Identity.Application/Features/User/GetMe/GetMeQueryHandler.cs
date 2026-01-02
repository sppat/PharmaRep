using Identity.Application.Dtos;
using Identity.Domain.DomainErrors;
using Microsoft.AspNetCore.Identity;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.GetMe;

public class GetMeQueryHandler(
    UserManager<Domain.Entities.User> userManager) : IRequestHandler<GetMeQuery, Result<MeDto>>
{
    public async Task<Result<MeDto>> HandleAsync(GetMeQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null) return Result<MeDto>.Failure([IdentityModuleDomainErrors.UserErrors.UserNotFound], ResultType.NotFound);

        var userRoles = await userManager.GetRolesAsync(user);

        var meDto = new MeDto(
            Id: user.Id,
            Email: user.Email,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Roles: userRoles);

        return Result<MeDto>.Success(meDto);
    }
}
