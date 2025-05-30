using Appointments.Domain.DomainErrors;
using Appointments.Domain.Exceptions;
using Appointments.Domain.ValueObjects;
using Shared.Domain;

namespace Appointments.Domain.Entities;

public class Appointment
{
    public Guid Id { get; private set; }
    public AppointmentDate Date { get; private set; }
    public AppointmentAddress Address { get; private set; }
    public AppointmentOrganizerId OrganizerId { get; private set; }
    public IEnumerable<AppointmentAttendeeId> AttendeeIds { get; private set; }

    private Appointment(AppointmentDate date,
        AppointmentAddress address,
        AppointmentOrganizerId organizerId,
        IEnumerable<AppointmentAttendeeId> attendeeIds)
    {
        Id = Guid.NewGuid();
        Date = date;
        Address = address;
        OrganizerId = organizerId;
        AttendeeIds = attendeeIds ?? [];
    }
    
    public static DomainResult<Appointment> Create(DateTime startDate, 
        DateTime endDate,
        string street,
        ushort number,
        uint zipCode,
        AppointmentOrganizerId organizerId,
        IEnumerable<AppointmentAttendeeId> attendeeIds)
    {
        var appointmentAddressIsValid = AppointmentAddress.TryCreate(street, number, zipCode, out var address);
        if (!appointmentAddressIsValid)
        {
            return AppointmentsModuleDomainErrors.AppointmentErrors.InvalidAddress;
        }
        
        var appointmentDateIsValid = AppointmentDate.TryCreate(startDate, endDate, out var date);
        if (!appointmentDateIsValid)
        {
            return AppointmentsModuleDomainErrors.AppointmentErrors.InvalidDate;
        }
        
        return new Appointment(date, address, organizerId, attendeeIds);
    }
}