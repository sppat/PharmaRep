using Appointments.Domain.Entities;
using Appointments.Domain.Exceptions.Appointment;

namespace Appointments.Domain.Tests.EntityTests;

public class AttendeeTests
{
	[Fact]
	public void Create_ValidUserAndAppointmentId_CreatesAttendee()
	{
		// Arrange
		var validUserId = Guid.NewGuid();
		var appointmentId = Guid.NewGuid();

		// Act
		var attendee = Attendee.Create(validUserId, appointmentId);

		// Assert
		Assert.Equal(validUserId, attendee?.UserId.Value);
		Assert.Equal(appointmentId, attendee?.AppointmentId.Value);
	}

	[Fact]
	public void Create_EmptyUserId_ReturnsError()
	{
		// Arrange
		var emptyUserId = Guid.Empty;
		var appointmentId = Guid.NewGuid();

		// Act & Assert
		Assert.Throws<EmptyUserIdException>(() => Attendee.Create(emptyUserId, appointmentId));
	}

	[Fact]
	public void Create_EmptyAppointmentId_ReturnsError()
	{
		// Arrange
		var validUserId = Guid.NewGuid();
		var emptyAppointmentId = Guid.Empty;

		// Act & Assert
		Assert.Throws<EmptyAppointmentIdException>(() => Attendee.Create(validUserId, emptyAppointmentId));
	}
}
