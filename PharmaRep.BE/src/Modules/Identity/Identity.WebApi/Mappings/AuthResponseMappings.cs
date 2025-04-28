using Identity.WebApi.Responses;

namespace Identity.WebApi.Mappings;

public static class AuthResponseMappings
{
    internal static LoginResponse ToLoginResponse(string token) => new(token);
    
    internal static RegisterResponse ToRegisterResponse(Guid userId) => new(userId);
}