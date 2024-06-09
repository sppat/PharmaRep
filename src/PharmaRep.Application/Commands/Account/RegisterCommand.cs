using MediatR;
using PharmaRep.Application.Responses.Account;

namespace PharmaRep.Application.Commands.Account;

public record RegisterCommand(string Username, string Password) : IRequest<RegisterResponse>;
