using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Application.Results;

namespace Identity.Application.Features.User.Register;

public class RegisterUserCommandHandler(UserManager<Domain.Entities.User> userManager) : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = Domain.Entities.User.Create(email: request.Email, firstName: request.FirstName, lastName: request.LastName);
        var registerResult = await userManager.CreateAsync(user, request.Password);
        if (!registerResult.Succeeded)
        {
            var errors = registerResult.Errors
                .Select(e => e.Description)
                .ToList();

            return Result<Guid>.Failure(errors, ResultType.ValidationError);
        }

        return Result<Guid>.Success(user.Id, ResultType.Created);
    }
}