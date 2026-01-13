using Identity.Application.Features.Auth.Login;
using Identity.Application.Features.Auth.Register;
using Identity.WebApi.Requests;

namespace Identity.WebApi.Mappings;

public static class AuthRequestMappings
{
	public static LoginCommand ToCommand(this LoginRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);

		return new LoginCommand(Email: request.Email, Password: request.Password);
	}

	public static RegisterCommand ToCommand(this RegisterRequest request)
	{
		ArgumentNullException.ThrowIfNull(request);

		return new RegisterCommand(FirstName: request.FirstName,
			LastName: request.LastName,
			Email: request.Email,
			Password: request.Password);
	}
}
