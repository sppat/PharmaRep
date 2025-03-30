namespace Identity.WebApi.Endpoints;

public static class IdentityModuleUrls
{
    private const string ModuleBaseUrl = "api/identity";

    public static class User
    {
        private const string UserBaseUrl = $"{ModuleBaseUrl}/users";

        public const string GetById = $"{UserBaseUrl}/{{userId:guid}}";
        public const string Register = $"{UserBaseUrl}/register";
    }
}