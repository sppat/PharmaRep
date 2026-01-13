namespace Identity.Domain.Exceptions.UserExceptions;

public class UserArgumentException(string message, string paramName) : ArgumentException(message, paramName);
