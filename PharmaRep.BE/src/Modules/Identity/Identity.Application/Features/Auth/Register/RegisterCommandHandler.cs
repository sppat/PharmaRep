using Microsoft.AspNetCore.Identity;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.Auth.Register;

public class RegisterCommandHandler(UserManager<Domain.Entities.User> userManager) : IRequestHandler<RegisterCommand, Result<Guid>>
{
    public async Task<Result<Guid>> HandleAsync(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = Domain.Entities.User.Create(email: request.Email,
            firstName: request.FirstName,
            lastName: request.LastName);
        
        var registerResult = await userManager.CreateAsync(user, request.Password);
        if (registerResult.Succeeded)
        {
            return Result<Guid>.Success(user.Id, ResultType.Created);
        }
        
        var registerErrors = registerResult.Errors
            .Select(e => e.Description)
            .ToList();
        
        return Result<Guid>.Failure(registerErrors, ResultType.ValidationError);
    }
}