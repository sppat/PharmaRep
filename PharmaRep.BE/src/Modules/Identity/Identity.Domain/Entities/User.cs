using System.Text.RegularExpressions;
using Identity.Domain.DomainErrors;
using Identity.Domain.Exceptions.UserExceptions;
using Identity.Domain.RegexConstants;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities;

public partial class User : IdentityUser<Guid>
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
            throw new UserArgumentException(DomainErrorsConstants.UserDomainErrors.InvalidEmail, nameof(email));
        }

        base.Id = Guid.NewGuid();
        base.Email = email;
        base.UserName = email;
        FirstName = firstName;
        LastName = lastName;
    }
    
    public static User Create(string email, string firstName, string lastName) => new(email, firstName, lastName);
}