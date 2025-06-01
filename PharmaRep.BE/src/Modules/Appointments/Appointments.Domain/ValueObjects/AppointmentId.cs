namespace Appointments.Domain.ValueObjects;

public record AppointmentId
{
    public Guid Value { get; private set; }
    
    private AppointmentId() { }

    private AppointmentId(Guid value)
    {
        Value = value;
    }
    
    public static bool TryCreate(Guid id, out AppointmentId appointmentId)
    {
        if (id == Guid.Empty)
        {
            appointmentId = null;
            return false;
        }

        appointmentId = new AppointmentId(id);
        return true;
    }
}