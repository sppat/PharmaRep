using Identity.Application.Features.User.UpdateRoles;
using Identity.Domain.Entities;

using Microsoft.AspNetCore.Identity;

using Moq;

using Shared.Application.Results;

namespace Identity.Application.Tests.CommandHandlers;

public class UpdateRolesCommandHandlerTests
{
	private readonly UpdateRolesCommandHandler _sut;
	private readonly Mock<UserManager<User>> _userManagerMock;

	public UpdateRolesCommandHandlerTests()
	{
		var userStoreMock = new Mock<IUserStore<User>>();
		_userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);
		_sut = new UpdateRolesCommandHandler(_userManagerMock.Object);
	}

	[Fact]
	public async Task HandleAsync_ValidCommand_ReturnsSuccessResult()
	{
		// Arrange
		var user = User.Create(email: "user@test.com", firstName: "user", lastName: "test");
		var roles = new[] { Role.Doctor.Name };
		var command = new UpdateRolesCommand(user.Id, roles);
		_userManagerMock.Setup(u => u.FindByIdAsync(user.Id.ToString()))
			.ReturnsAsync(user);
		_userManagerMock.Setup(u => u.GetRolesAsync(user))
			.ReturnsAsync([Role.MedicalRepresentative.Name]);
		_userManagerMock.Setup(u => u.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
			.ReturnsAsync(IdentityResult.Success);
		_userManagerMock.Setup(u => u.AddToRolesAsync(user, roles))
			.ReturnsAsync(IdentityResult.Success);

		// Act
		var result = await _sut.HandleAsync(command, CancellationToken.None);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(ResultType.Updated, result.Type);
	}

	[Fact]
	public async Task HandleAsync_UserNotExist_ReturnsNotFoundResult()
	{
		// Arrange
		var roles = new[] { Role.Doctor.Name };
		var command = new UpdateRolesCommand(Guid.Empty, roles);
		_userManagerMock.Setup(u => u.FindByIdAsync(Guid.Empty.ToString()))
			.ReturnsAsync((User)null);

		// Act
		var result = await _sut.HandleAsync(command, CancellationToken.None);

		// Assert
		Assert.False(result.IsSuccess);
		Assert.Equal(ResultType.NotFound, result.Type);
	}

	[Fact]
	public async Task HandleAsync_AddToRoleError_ReturnsServerErrorResult()
	{
		// Arrange
		var user = User.Create(email: "user@test.com", firstName: "user", lastName: "test");
		var roles = new[] { Role.Doctor.Name };
		var command = new UpdateRolesCommand(user.Id, roles);
		_userManagerMock.Setup(u => u.FindByIdAsync(user.Id.ToString()))
			.ReturnsAsync(user);
		_userManagerMock.Setup(u => u.GetRolesAsync(user))
			.ReturnsAsync([Role.Doctor.Name]);
		_userManagerMock.Setup(u => u.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
			.ReturnsAsync(IdentityResult.Success);
		_userManagerMock.Setup(u => u.AddToRolesAsync(user, It.IsAny<IEnumerable<string>>()))
			.ReturnsAsync(IdentityResult.Failed([new IdentityError { Description = "Mock error" }]));

		// Act
		var result = await _sut.HandleAsync(command, CancellationToken.None);

		// Assert
		Assert.False(result.IsSuccess);
		Assert.Equal(ResultType.ServerError, result.Type);
	}

	[Fact]
	public async Task HandleAsync_RemoveFromRolesError_ReturnsServerErrorResult()
	{
		// Arrange
		var user = User.Create(email: "user@test.com", firstName: "user", lastName: "test");
		var roles = new[] { Role.Doctor.Name };
		var command = new UpdateRolesCommand(user.Id, roles);
		_userManagerMock.Setup(u => u.FindByIdAsync(user.Id.ToString()))
			.ReturnsAsync(user);
		_userManagerMock.Setup(u => u.GetRolesAsync(user))
			.ReturnsAsync([Role.Admin.Name]);
		_userManagerMock.Setup(u => u.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
			.ReturnsAsync(IdentityResult.Failed([new IdentityError { Description = "Mock error" }]));

		// Act
		var result = await _sut.HandleAsync(command, CancellationToken.None);

		// Assert
		Assert.False(result.IsSuccess);
		Assert.Equal(ResultType.ServerError, result.Type);
	}
}
