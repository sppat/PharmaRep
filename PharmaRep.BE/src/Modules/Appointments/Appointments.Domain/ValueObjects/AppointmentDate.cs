namespace Appointments.Domain.ValueObjects;

public record AppointmentDate
{
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }

    private AppointmentDate() { }
    
    private AppointmentDate(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public static bool TryCreate(DateTimeOffset startDate, DateTimeOffset endDate, out AppointmentDate appointmentDate)
    {
        if (startDate == default || endDate == default || startDate > endDate)
        {
            appointmentDate = null;
            return false;
        }

        appointmentDate = new AppointmentDate(startDate, endDate);
        return true;
    }
}