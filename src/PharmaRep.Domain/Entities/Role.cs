using Microsoft.AspNetCore.Identity;
using PharmaRep.Domain.Exceptions;

namespace PharmaRep.Domain;

public class Role : IdentityRole
{
    public Role(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new InvalidRoleNameException();
        }

        Name = name;
    }
}
