using FluentValidation;
using Identity.Domain.DomainErrors;
using Identity.Domain.Entities;
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
            .NotNull()
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
            .NotNull()
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
            .NotNull()
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

        RuleFor(req => req.Roles)
            .Must(RoleExist)
            .When(req => req.Roles is not null)
            .WithMessage(DomainErrorsConstants.UserDomainErrors.InvalidRole);

        #endregion
    }

    private static bool RoleExist(IEnumerable<string> roles)
    {
        var existingRoles = Role.All.Select(r => r.Name)
            .ToList();

        foreach (var role in roles)
        {
            var roleExist = existingRoles.Contains(role, StringComparer.InvariantCultureIgnoreCase);
            
            if (!roleExist) return false;
        }

        return true;
    }
}