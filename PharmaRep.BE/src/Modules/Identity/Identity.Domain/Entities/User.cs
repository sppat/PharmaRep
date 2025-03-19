using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public ICollection<UserRole> UserRoles { get; private set; }
    
    public static User Create(string email, string firstName, string lastName) => new()
    {
        Id = Guid.NewGuid(),
        Email = email,
        UserName = email,
        FirstName = firstName,
        LastName = lastName
    };
}