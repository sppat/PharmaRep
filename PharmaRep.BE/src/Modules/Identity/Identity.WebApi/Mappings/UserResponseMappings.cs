using Identity.WebApi.Responses;

namespace Identity.WebApi.Mappings;

public static class UserResponseMappings
{
    internal static RegisterUserResponse ToRegisterUserResponse(Guid userId) => new(userId);
}