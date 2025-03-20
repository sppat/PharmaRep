using System.Text.RegularExpressions;
using Identity.Domain.Exceptions.UserExceptions;
using Identity.Domain.RegexConstants;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities;

public partial class User : IdentityUser<Guid>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public ICollection<UserRole> UserRoles { get; private set; }
    
    private User()
    {
        
    }

    private User(string email, string firstName, string lastName)
    {
        if (!UserRegex.EmailFormat().IsMatch(email))
        {
            throw new InvalidUserEmailException();
        }
    }
    
    public static User Create(string email, string firstName, string lastName) => new()
    {
        Id = Guid.NewGuid(),
        Email = email,
        UserName = email,
        FirstName = firstName,
        LastName = lastName
    };
}