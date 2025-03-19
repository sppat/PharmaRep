using Identity.Domain.DomainErrors;

namespace Identity.Domain.Exceptions.UserExceptions;

public class InvalidUserFirstNameException() : Exception(DomainErrorsConstants.UserDomainErrors.InvalidFirstName)
{
    
}