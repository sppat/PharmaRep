using Identity.Domain.DomainErrors;

namespace Identity.Domain.Exceptions.UserExceptions;

public class InvalidUserLastNameException() : Exception(DomainErrorsConstants.UserDomainErrors.InvalidLastName)
{
    
}