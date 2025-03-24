using FluentValidation;
using Identity.Domain.DomainErrors;
using Identity.Domain.RegexConstants;

namespace Identity.Application.Features.User.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private const int MaxNameLength = 50;
    private const int MaxEmailLength = 100;
    
    public RegisterUserCommandValidator()
    {
        #region FirstName

        RuleFor(req => req.FirstName)
            .NotEmpty()
            .WithMessage(DomainErrorsConstants.UserDomainErrors.InvalidFirstName);
        
        When(req => req.FirstName is not null, () =>
        {
            RuleFor(req => req.FirstName)
                .MaximumLength(MaxNameLength)
                .WithMessage(DomainErrorsConstants.UserDomainErrors.NameOutOfRange);
            
            RuleFor(req => req.FirstName)
                .Must(UserRegex.NameFormat().IsMatch)
                .WithMessage(DomainErrorsConstants.UserDomainErrors.InvalidFirstName);
        });

        #endregion

        #region LastName

        RuleFor(req => req.LastName)
            .NotEmpty()
            .WithMessage(DomainErrorsConstants.UserDomainErrors.InvalidLastName);
        
        When(req => req.LastName is not null, () =>
        {
            RuleFor(req => req.LastName)
                .MaximumLength(MaxNameLength)
                .WithMessage(DomainErrorsConstants.UserDomainErrors.NameOutOfRange);
            
            RuleFor(req => req.LastName)
                .Must(UserRegex.NameFormat().IsMatch)
                .WithMessage(DomainErrorsConstants.UserDomainErrors.InvalidLastName);
        });

        #endregion

        #region Email

        RuleFor(req => req.Email)
            .NotEmpty()
            .WithMessage(DomainErrorsConstants.UserDomainErrors.InvalidEmail);

        When(req => req.Email is not null, () =>
        {
            RuleFor(req => req.Email)
                .MaximumLength(MaxEmailLength)
                .WithMessage(DomainErrorsConstants.UserDomainErrors.EmailOutOfRange);
            
            RuleFor(req => req.Email)
                .Must(UserRegex.EmailFormat().IsMatch)
                .WithMessage(DomainErrorsConstants.UserDomainErrors.InvalidEmail);
        });

        #endregion

        #region Password

        RuleFor(req => req.Password)
            .NotEmpty()
            .WithMessage(DomainErrorsConstants.UserDomainErrors.InvalidPassword);

        #endregion
        
        #region Roles
        
        RuleFor(req => req.Roles)
            .NotEmpty()
            .WithMessage(DomainErrorsConstants.UserDomainErrors.EmptyRoles);
        
        #endregion
    }
}