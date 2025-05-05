using FluentValidation;

namespace Identity.Application.Features.User.Delete;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(cmd => cmd.UserId).NotEmpty();
    }
}