using Appointments.Domain.Exceptions;

namespace Appointments.Domain.ValueObjects;

public record AppointmentOrganizerId
{
    public Guid Value { get; }

    private AppointmentOrganizerId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new AppointmentEmptyOrganizerIdException();
        }

        Value = value;
    }

    public static implicit operator AppointmentOrganizerId(Guid id) => new(id);
    public static implicit operator Guid(AppointmentOrganizerId appointmentOrganizerId) => appointmentOrganizerId.Value;
}