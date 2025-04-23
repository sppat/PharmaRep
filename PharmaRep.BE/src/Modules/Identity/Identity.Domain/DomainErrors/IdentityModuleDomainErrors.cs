namespace Identity.Domain.DomainErrors;

public static class IdentityModuleDomainErrors
{
    public static class UserErrors
    {
        public const string InvalidEmail = "Invalid user email";
        public const string InvalidFirstName = "Invalid user first name";
        public const string InvalidLastName = "Invalid user last name";
        public const string NameOutOfRange = "Name length is out of range";
        public const string EmailOutOfRange = "Email length is out of range";
        public const string InvalidPassword = "Invalid password";
        public const string EmptyRoles = "Empty roles";
        public const string InvalidRole = "Invalid role";
        public const string UserNotFound = "User not found";
        public const string InvalidCredentials = "Email or password is incorrect";
    }
}