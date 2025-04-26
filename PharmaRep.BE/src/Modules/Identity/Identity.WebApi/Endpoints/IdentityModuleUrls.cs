namespace Identity.WebApi.Endpoints;

public static class IdentityModuleUrls
{
    internal const string ModuleBaseUrl = "api/identity";

    public static class User
    {
        private const string UserBaseUrl = $"users";

        public const string GetAll = UserBaseUrl;
        public const string GetById = $"{UserBaseUrl}/{{id:guid}}";
        public const string Login = $"{UserBaseUrl}/login";
        public const string Register = $"{UserBaseUrl}/register";
    }
}