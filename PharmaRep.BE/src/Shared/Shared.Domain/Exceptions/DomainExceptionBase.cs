namespace Shared.Domain.Exceptions;

public class DomainExceptionBase(string message, string? param = null) : ArgumentException(message, param);
