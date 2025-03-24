namespace Identity.Domain.DomainErrors;

public static class DomainErrorsConstants
{
    public static class UserDomainErrors
    {
        public const string InvalidEmail = "Invalid user email";
        public const string InvalidFirstName = "Invalid user first name";
        public const string InvalidLastName = "Invalid user last name";
        public const string NameOutOfRange = "Name is out of range";
        public const string EmailOutOfRange = "Email is out of range";
        public const string InvalidPassword = "Invalid password";
        public const string EmptyRoles = "Empty roles";
    }
}