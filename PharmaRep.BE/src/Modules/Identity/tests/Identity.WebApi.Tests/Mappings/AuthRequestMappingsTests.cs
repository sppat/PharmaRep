using Identity.WebApi.Mappings;
using Identity.WebApi.Requests;

namespace Identity.WebApi.Tests.Mappings;

public class AuthRequestMappingsTests
{
	#region ToLoginCommand

	[Fact]
	public void ToLoginCommand_NotNullRequest_ReturnsCommand()
	{
		// Arrange
		var request = new LoginRequest(Email: "test@email.com", Password: "password");

		// Act
		var command = request.ToCommand();

		// Assert
		Assert.NotNull(command);
		Assert.Equal(request.Email, command.Email);
		Assert.Equal(request.Password, command.Password);
	}

	[Fact]
	public void ToLoginCommand_NullRequest_ThrowsException()
	{
		// Act
		void Act() => ((LoginRequest)null)!.ToCommand();

		// Assert
		Assert.Throws<ArgumentNullException>(Act);
	}

	#endregion

	#region ToRegisterCommand

	[Fact]
	public void ToRegisterCommand_NotNullRequest_ReturnsCommand()
	{
		// Arrange
		var request = new RegisterRequest(FirstName: "John",
			LastName: "Doe",
			Email: "john@doe.com",
			Password: "password");

		// Act
		var command = request.ToCommand();

		// Assert
		Assert.NotNull(command);
		Assert.Equal(request.Email, command.Email);
		Assert.Equal(request.Password, command.Password);
		Assert.Equal(request.FirstName, command.FirstName);
		Assert.Equal(request.LastName, command.LastName);
	}

	[Fact]
	public void ToRegisterCommand_NullRequest_ThrowsException()
	{
		// Act
		void Act() => ((RegisterRequest)null).ToCommand();

		// Assert
		Assert.Throws<ArgumentNullException>(Act);
	}

	#endregion
}
