using Identity.Application.Dtos;
using Identity.Application.Features.User.GetMe;
using Identity.Domain.DomainErrors;
using Identity.Domain.Entities;

using Microsoft.AspNetCore.Identity;

using Moq;

using Shared.Application.Results;
using Shared.Tests;

namespace Identity.Application.Tests.QueryHandlers;

public class GetMeQueryHandlerTests
{
	private readonly Mock<UserManager<Domain.Entities.User>> _userManagerMock;
	private readonly GetMeQueryHandler _sut;

	public GetMeQueryHandlerTests()
	{
		var userStoreMock = new Mock<IUserStore<User>>();
		_userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

		_sut = new GetMeQueryHandler(_userManagerMock.Object);
	}

	[Fact]
	public async Task HandleAsync_UserNotFound_ReturnsNotFoundResult()
	{
		// Arrange
		var query = new GetMeQuery(Guid.Empty);

		_userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
			.ReturnsAsync((User)null);

		// Act
		var result = await _sut.HandleAsync(query, CancellationToken.None);

		// Assert
		Assert.False(result.IsSuccess);
		Assert.Equal(ResultType.NotFound, result.Type);
		AssertProblemDetails.HasErrors([IdentityModuleDomainErrors.UserErrors.UserNotFound], result.Errors);
	}

	[Fact]
	public async Task HandleAsync_UserFound_ReturnsSuccessResultWithMeDto()
	{
		// Arrange
		var firstName = "John";
		var lastName = "Doe";
		var email = "john.doe@example.com";
		var roles = new[] { Role.Doctor.Name };
		var exptectedMeDto = new MeDto(
			Id: Guid.Empty,
			Email: email,
			FirstName: firstName,
			LastName: lastName,
			Roles: roles);
		var query = new GetMeQuery(Guid.Empty);

		_userManagerMock.Setup(u => u.FindByIdAsync(Guid.Empty.ToString()))
			.ReturnsAsync(User.Create(email: email, firstName: firstName, lastName: lastName));

		_userManagerMock.Setup(u => u.GetRolesAsync(It.IsAny<User>()))
			.ReturnsAsync(roles);

		// Act
		var result = await _sut.HandleAsync(query, CancellationToken.None);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(ResultType.Success, result.Type);

		Assert.Equal(exptectedMeDto.Email, result.Value.Email);
		Assert.Equal(exptectedMeDto.FirstName, result.Value.FirstName);
		Assert.Equal(exptectedMeDto.LastName, result.Value.LastName);
		Assert.Equal(exptectedMeDto.Roles, result.Value.Roles);
	}
}
