using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities;

public class UserRole : IdentityUserRole<Guid>
{
    public User User { get; private set; }
    public Role Role { get; private set; }
}