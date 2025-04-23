using Identity.Application.Interfaces;
using Identity.Domain.DomainErrors;
using Microsoft.AspNetCore.Identity;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.Login;

public class LoginUserCommandHandler(IAuthHandler authHandler,
    UserManager<Domain.Entities.User> userManager,
    SignInManager<Domain.Entities.User> signInManager) : IRequestHandler<LoginUserCommand, Result<string>>
{
    public async Task<Result<string>> HandleAsync(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            var errors = new[] { IdentityModuleDomainErrors.UserErrors.UserNotFound };
            return Result<string>.Failure(errors, ResultType.NotFound);
        }
        
        var signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!signInResult.Succeeded)
        {
            var errors = new[] { IdentityModuleDomainErrors.UserErrors.InvalidCredentials };
            return Result<string>.Failure(errors, ResultType.ValidationError);
        }
        
        var userRoles = await userManager.GetRolesAsync(user);
        var token = authHandler.GenerateToken(user.Id.ToString(), user.Email, userRoles);
        
        return Result<string>.Success(token);
    }
}