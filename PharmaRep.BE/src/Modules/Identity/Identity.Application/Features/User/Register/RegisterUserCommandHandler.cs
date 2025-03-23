using MediatR;
using Shared.Application.Results;

namespace Identity.Application.Features.User.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    public Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}