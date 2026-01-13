using Appointments.Domain.Exceptions.Appointment;
using Appointments.Domain.ValueObjects;

namespace Appointments.Domain.Tests.ValueObjectsTests;

public class UserIdTests
{
	[Fact]
	public void TryCreate_ValidId_CreatesUserId()
	{
		// Arrange
		var validId = Guid.NewGuid();

		// Act
		var userId = new UserId(validId);

		// Assert
		Assert.Equal(validId, userId.Value);
	}

	[Fact]
	public void TryCreate_EmptyId_ThrowsException()
	{
		// Act
		Assert.Throws<EmptyUserIdException>(() => new UserId(Guid.Empty));
	}
}
