namespace PharmaRep.Domain.Exceptions;

public class DatabaseMigrationException(string message) : Exception(message);