using MediatR;
using PharmaRep.Application.Commands.Account;
using PharmaRep.Application.Responses.Account;

namespace PharmaRep.Application.CommandHandlers.Account;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    public Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new RegisterResponse());
    }
}