using Identity.WebApi.Responses;
using Shared.Application.Results;

namespace Identity.WebApi.Mappings;

public static class UserResponseMappings
{
    internal static RegisterUserResponse ToRegisterUserResponse(Guid userId) => new(userId);
}