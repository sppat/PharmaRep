using Appointments.Domain.Exceptions.Appointment;
using Appointments.Domain.ValueObjects;

namespace Appointments.Domain.Tests.ValueObjectsTests;

public class AppointmentIdTests
{
	[Fact]
	public void TryCreate_ValidId_CreatesAppointmentId()
	{
		// Arrange
		var validId = Guid.NewGuid();

		// Act
		var appointmentId = new AppointmentId(validId);

		// Assert
		Assert.Equal(validId, appointmentId.Value);
	}

	[Fact]
	public void TryCreate_EmptyId_ReturnsFalse()
	{
		// Arrange
		var emptyId = Guid.Empty;

		// Act & Assert
		Assert.Throws<EmptyAppointmentIdException>(() => new AppointmentId(Guid.Empty));
	}
}
