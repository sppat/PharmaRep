﻿using Identity.Domain.DomainErrors;
using Microsoft.AspNetCore.Identity;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.Delete;

public class DeleteUserCommandHandler(UserManager<Domain.Entities.User> userManager) : IRequestHandler<DeleteUserCommand, Result>
{
    public async Task<Result> HandleAsync(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            var errors = new[] { IdentityModuleDomainErrors.UserErrors.UserNotFound };
            
            return Result.Failure(errors, ResultType.NotFound);
        }
        
        var result = await userManager.DeleteAsync(user);
        if (result.Succeeded) return Result.Success(ResultType.Deleted);
        
        var identityResultErrors = result.Errors.Select(e => e.Description).ToArray();
        
        return Result.Failure(identityResultErrors, ResultType.ServerError);
    }
}