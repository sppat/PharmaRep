using Identity.Domain.DomainErrors;
using Identity.Domain.Exceptions.UserExceptions;
using Identity.Domain.RegexConstants;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public ICollection<UserRole> UserRoles { get; private set; } = [];
    
    private User()
    {
        
    }

    private User(string email, string firstName, string lastName)
    {
        if (email is null || !UserRegex.EmailFormat().IsMatch(email))
        {
            throw new UserArgumentException(IdentityModuleDomainErrors.UserErrors.InvalidEmail, nameof(email));
        }

        if (firstName is null || !UserRegex.NameFormat().IsMatch(firstName))
        {
            throw new UserArgumentException(IdentityModuleDomainErrors.UserErrors.InvalidFirstName, nameof(firstName));
        }
        
        if (lastName is null || !UserRegex.NameFormat().IsMatch(lastName))
        {
            throw new UserArgumentException(IdentityModuleDomainErrors.UserErrors.InvalidLastName, nameof(lastName));
        }

        base.Id = Guid.NewGuid();
        base.Email = email;
        base.UserName = email;
        FirstName = firstName;
        LastName = lastName;
    }
    
    public void UpdateFirstName(string firstName)
    {
        if (firstName is null || !UserRegex.NameFormat().IsMatch(firstName))
        {
            throw new UserArgumentException(IdentityModuleDomainErrors.UserErrors.InvalidFirstName, nameof(firstName));
        }

        FirstName = firstName;
    }
    
    public void UpdateLastName(string lastName)
    {
        if (lastName is null || !UserRegex.NameFormat().IsMatch(lastName))
        {
            throw new UserArgumentException(IdentityModuleDomainErrors.UserErrors.InvalidFirstName, nameof(lastName));
        }

        LastName = lastName;
    }
    
    public static User Create(string email, string firstName, string lastName) => new(email, firstName, lastName);
}