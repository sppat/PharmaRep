using FluentValidation;
using Identity.Domain.DomainErrors;
using Identity.Domain.Entities;
using Identity.Domain.RegexConstants;

namespace Identity.Application.Features.Auth.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private const int MaxNameLength = 50;
    private const int MaxEmailLength = 100;
    
    public RegisterCommandValidator()
    {
        #region FirstName

        RuleFor(req => req.FirstName)
            .NotNull()
            .WithMessage(IdentityModuleDomainErrors.UserErrors.InvalidFirstName);
        
        When(req => req.FirstName is not null, () =>
        {
            RuleFor(req => req.FirstName)
                .MaximumLength(MaxNameLength)
                .WithMessage(IdentityModuleDomainErrors.UserErrors.NameOutOfRange);
            
            RuleFor(req => req.FirstName)
                .Must(UserRegex.NameFormat().IsMatch)
                .WithMessage(IdentityModuleDomainErrors.UserErrors.InvalidFirstName);
        });

        #endregion

        #region LastName

        RuleFor(req => req.LastName)
            .NotNull()
            .WithMessage(IdentityModuleDomainErrors.UserErrors.InvalidLastName);
        
        When(req => req.LastName is not null, () =>
        {
            RuleFor(req => req.LastName)
                .MaximumLength(MaxNameLength)
                .WithMessage(IdentityModuleDomainErrors.UserErrors.NameOutOfRange);
            
            RuleFor(req => req.LastName)
                .Must(UserRegex.NameFormat().IsMatch)
                .WithMessage(IdentityModuleDomainErrors.UserErrors.InvalidLastName);
        });

        #endregion

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