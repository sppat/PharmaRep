using FluentValidation;
using Identity.Domain.DomainErrors;
using Identity.Domain.RegexConstants;

namespace Identity.Application.Features.Auth.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginCommand>
{
    private const int MaxEmailLength = 100;
    
    public LoginUserCommandValidator()
    {
        #region Email

        RuleFor(req => req.Email)
            .NotNull()
            .WithMessage(IdentityModuleDomainErrors.UserErrors.InvalidEmail);

        When(req => req.Email is not null, () =>
        {
            RuleFor(req => req.Email)
                .MaximumLength(MaxEmailLength)
                .WithMessage(IdentityModuleDomainErrors.UserErrors.EmailOutOfRange);
            
            RuleFor(req => req.Email)
                .Must(UserRegex.EmailFormat().IsMatch)
                .WithMessage(IdentityModuleDomainErrors.UserErrors.InvalidEmail);
        });

        #endregion
        
        #region Password
        
        RuleFor(req => req.Password)
            .NotEmpty()
            .WithMessage(IdentityModuleDomainErrors.UserErrors.InvalidPassword);
        
        #endregion
    }
}