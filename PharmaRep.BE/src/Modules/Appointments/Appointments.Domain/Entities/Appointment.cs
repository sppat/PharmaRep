using Appointments.Domain.DomainErrors;
using Appointments.Domain.Exceptions;
using Appointments.Domain.ValueObjects;

namespace Appointments.Domain.Entities;

public class Appointment
{
    public Guid Id { get; private set; }
    public AppointmentDate StartDate { get; private set; }
    public AppointmentDate EndDate { get; private set; }
    public AppointmentAddress Address { get; private set; }
    public Guid Organizer { get; private set; }
    public IEnumerable<Guid> Attendees { get; private set; }

    private Appointment(AppointmentDate startDate, AppointmentDate endDate, AppointmentAddress address, Guid organizer, IEnumerable<Guid> attendees)
    {
        if (startDate.Value > endDate.Value)
        {
            throw new AppointmentDateException(AppointmentsModuleDomainErrors.AppointmentErrors.InvalidEndDate);
        }
        
        Id = Guid.NewGuid();
        StartDate = startDate;
        EndDate = endDate;
        Address = address;
        Organizer = organizer;
        Attendees = attendees ?? [];
    }
    
    public static Appointment Create(AppointmentDate startDate, AppointmentDate endDate, AppointmentAddress address, Guid organizer, IEnumerable<Guid> attendees)
    {
        return new Appointment(startDate, endDate, address, organizer, attendees);
    }
}