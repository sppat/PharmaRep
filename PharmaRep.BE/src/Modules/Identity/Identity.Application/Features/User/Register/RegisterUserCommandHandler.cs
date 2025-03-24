using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Application.Results;

namespace Identity.Application.Features.User.Register;

public class RegisterUserCommandHandler(UserManager<Domain.Entities.User> userManager) : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = Domain.Entities.User.Create(email: request.Email,
            firstName: request.FirstName,
            lastName: request.LastName);
        
        var registerResult = await userManager.CreateAsync(user, request.Password);
        if (!registerResult.Succeeded)
        {
            var registerErrors = GetIdentityResultErrors(registerResult);
            
            return Result<Guid>.Failure(registerErrors, ResultType.ValidationError);
        }

        var addToRoleResult = await userManager.AddToRolesAsync(user, request.Roles);
        if (addToRoleResult.Succeeded)
        {
            return Result<Guid>.Success(user.Id, ResultType.Created);
        }

        var addToRoleErrors = GetIdentityResultErrors(addToRoleResult);
        
        return Result<Guid>.Failure(addToRoleErrors, ResultType.ValidationError);
    }
    
    private static List<string> GetIdentityResultErrors(IdentityResult result) => result.Errors
        .Select(e => e.Description)
        .ToList();
}