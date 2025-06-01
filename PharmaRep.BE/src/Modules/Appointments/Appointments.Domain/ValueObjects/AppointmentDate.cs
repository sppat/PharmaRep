namespace Appointments.Domain.ValueObjects;

public record AppointmentDate
{
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    private AppointmentDate() { }
    
    private AppointmentDate(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public static bool TryCreate(DateTime startDate, DateTime endDate, out AppointmentDate appointmentDate)
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