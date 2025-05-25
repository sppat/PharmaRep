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
    public AppointmentOrganizerId OrganizerId { get; private set; }
    public IEnumerable<AppointmentAttendeeId> AttendeeIds { get; private set; }

    private Appointment(AppointmentDate startDate, 
        AppointmentDate endDate,
        AppointmentAddress address,
        AppointmentOrganizerId organizerId,
        IEnumerable<AppointmentAttendeeId> attendeeIds)
    {
        if (startDate.Value > endDate.Value)
        {
            throw new AppointmentDateException(AppointmentsModuleDomainErrors.AppointmentErrors.InvalidEndDate);
        }

        Id = Guid.NewGuid();
        StartDate = startDate;
        EndDate = endDate;
        Address = address;
        OrganizerId = organizerId;
        AttendeeIds = attendeeIds ?? [];
    }
    
    public static Appointment Create(AppointmentDate startDate, 
        AppointmentDate endDate,
        AppointmentAddress address,
        AppointmentOrganizerId organizerId,
        IEnumerable<AppointmentAttendeeId> attendeeIds)
    {
        return new Appointment(startDate, endDate, address, organizerId, attendeeIds);
    }
}