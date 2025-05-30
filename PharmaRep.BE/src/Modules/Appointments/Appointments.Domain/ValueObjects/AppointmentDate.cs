using Appointments.Domain.DomainErrors;
using Appointments.Domain.Exceptions;

namespace Appointments.Domain.ValueObjects;

public record AppointmentDate
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    
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