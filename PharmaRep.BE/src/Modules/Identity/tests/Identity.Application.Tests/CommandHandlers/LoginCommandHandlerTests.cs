using Identity.Application.Features.Auth.Login;
using Identity.Application.Interfaces;
using Identity.Domain.DomainErrors;
using Identity.Domain.Entities;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Moq;

using Shared.Application.Results;

namespace Identity.Application.Tests.CommandHandlers;

public class LoginCommandHandlerTests
{
	private readonly LoginCommandHandler _handler;
	private readonly Mock<IAuthHandler> _authHandlerMock;
	private readonly Mock<UserManager<User>> _userManagerMock;
	private readonly Mock<SignInManager<User>> _signInManagerMock;

	public LoginCommandHandlerTests()
	{
		var userStoreMock = new Mock<IUserStore<User>>();
		var contextAccessorMock = new Mock<IHttpContextAccessor>();
		var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<User>>();
		var optionsMock = new Mock<IOptions<IdentityOptions>>();
		var loggerMock = new Mock<ILogger<SignInManager<User>>>();
		var schemesMock = new Mock<IAuthenticationSchemeProvider>();
		var confirmationMock = new Mock<IUserConfirmation<User>>();

		_userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);
		_signInManagerMock = new Mock<SignInManager<User>>(_userManagerMock.Object,
			contextAccessorMock.Object,
			claimsFactoryMock.Object,
			optionsMock.Object,
			loggerMock.Object,
			schemesMock.Object,
			confirmationMock.Object);
		_authHandlerMock = new Mock<IAuthHandler>();
		_handler = new LoginCommandHandler(_authHandlerMock.Object,
			_userManagerMock.Object,
			_signInManagerMock.Object);
	}

	[Fact]
	public async Task HandleAsync_ValidCommand_ReturnsSuccessResult()
	{
		// Arrange
		var user = User.Create(email: "user@one.com", firstName: "user", lastName: "one");
		_userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
			.ReturnsAsync(user);

		_signInManagerMock.Setup(s => s.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
			.ReturnsAsync(SignInResult.Success);

		_userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
			.ReturnsAsync([]);

		const string expectedToken = "expected_token";
		_authHandlerMock.Setup(a => a.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
			.Returns(expectedToken);

		var command = new LoginCommand(Email: user.Email, Password: "password");

		// Act
		var result = await _handler.HandleAsync(command, CancellationToken.None);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(expectedToken, result.Value);
	}

	[Fact]
	public async Task HandleAsync_UserNotExist_ReturnsNotFoundResult()
	{
		// Arrange
		_userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
			.ReturnsAsync((User)null);

		const string expectedToken = "expected_token";
		_authHandlerMock.Setup(a => a.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
			.Returns(expectedToken);

		var command = new LoginCommand(Email: string.Empty, Password: string.Empty);

		// Act
		var result = await _handler.HandleAsync(command, CancellationToken.None);

		// Assert
		Assert.False(result.IsSuccess);
		Assert.Equal(ResultType.NotFound, result.Type);
		Assert.Contains(IdentityModuleDomainErrors.UserErrors.UserNotFound, result.Errors);
	}

	[Fact]
	public async Task HandleAsync_WrongPassword_ReturnsValidationResult()
	{
		// Arrange
		var user = User.Create(email: "user@one.com", firstName: "user", lastName: "one");
		_userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
			.ReturnsAsync(user);

		_signInManagerMock.Setup(s => s.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
			.ReturnsAsync(SignInResult.Failed);

		var command = new LoginCommand(Email: user.Email, Password: "password");

		// Act
		var result = await _handler.HandleAsync(command, CancellationToken.None);

		// Assert
		Assert.False(result.IsSuccess);
		Assert.Equal(ResultType.ValidationError, result.Type);
		Assert.Contains(IdentityModuleDomainErrors.UserErrors.InvalidCredentials, result.Errors);
	}
}
