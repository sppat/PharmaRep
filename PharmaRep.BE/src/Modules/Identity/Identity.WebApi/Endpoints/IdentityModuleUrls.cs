namespace Identity.WebApi.Endpoints;

public static class IdentityModuleUrls
{
    private const string ModuleBaseUrl = "api/identity";

    public static class User
    {
        private const string UserBaseUrl = $"{ModuleBaseUrl}/users";

        public const string GetAll = UserBaseUrl;
        public const string GetById = $"{UserBaseUrl}/{{id:guid}}";
        public const string UpdateRoles = $"{UserBaseUrl}/{{id:guid}}/roles";
        public const string Delete = $"{UserBaseUrl}/{{id:guid}}";
    }

    public static class Authentication
    {
        private const string AuthenticationBaseUrl = $"{ModuleBaseUrl}/auth";
        
        public const string Login = $"{AuthenticationBaseUrl}/login";
        public const string Register = $"{AuthenticationBaseUrl}/register";
    }
}