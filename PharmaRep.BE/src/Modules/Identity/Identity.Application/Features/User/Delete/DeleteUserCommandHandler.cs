using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Identity.Application.Features.User.Delete;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    public Task<Result> HandleAsync(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}