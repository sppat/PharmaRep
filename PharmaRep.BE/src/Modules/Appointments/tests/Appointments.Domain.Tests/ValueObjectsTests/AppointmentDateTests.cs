using Appointments.Domain.Exceptions.Appointment;
using Appointments.Domain.ValueObjects;

namespace Appointments.Domain.Tests.ValueObjectsTests;

public class AppointmentDateTests
{
	private readonly DateTime _validDate = DateTime.UtcNow;

	[Fact]
	public void AppointmentDate_ValidDate_CreatesAppointmentDate()
	{
		// Arrange
		// Act
		var appointmentDate = new AppointmentDate(_validDate);

		// Assert
		Assert.Equal(_validDate, appointmentDate.Value);
	}

	[Fact]
	public void AppointmentDate_DefaultDate_ThrowsException()
	{
		// Act & Assert
		Assert.Throws<EmptyAppointmentDateException>(() => new AppointmentDate(default));
	}
}
