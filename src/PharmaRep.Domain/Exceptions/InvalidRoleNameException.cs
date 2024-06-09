namespace PharmaRep.Domain.Exceptions;

public class InvalidRoleNameException : Exception
{
    public InvalidRoleNameException() : base("Invalid role name")
    {
        
    }
}