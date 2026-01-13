using Appointments.Domain.Exceptions.Appointment;

namespace Appointments.Domain.ValueObjects;

public record UserId
{
	public Guid Value { get; private set; }

	public UserId(Guid value)
	{
		if (value == Guid.Empty)
		{
			throw new EmptyUserIdException(nameof(UserId));
		}

		Value = value;
	}

	public static implicit operator Guid(UserId userId) => userId.Value;
	public static implicit operator UserId(Guid value) => new(value);
}
