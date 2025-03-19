using Identity.Domain.DomainErrors;

namespace Identity.Domain.Exceptions.UserExceptions;

public class InvalidUserEmailException() : Exception(DomainErrorsConstants.UserDomainErrors.InvalidEmail)
{
    
}