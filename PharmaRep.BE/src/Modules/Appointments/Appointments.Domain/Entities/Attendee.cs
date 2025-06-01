using Appointments.Domain.DomainErrors;
using Appointments.Domain.ValueObjects;
using Shared.Domain;

namespace Appointments.Domain.Entities;

public class Attendee
{
    public UserId UserId { get; private set; }
    public AppointmentId AppointmentId { get; private set; }
    
    private Attendee() { }

    private Attendee(UserId userId, AppointmentId appointmentId)
    {
        UserId = userId;
        AppointmentId = appointmentId;
    }
    
    public static DomainResult<Attendee> Create(Guid userId, Guid appointmentId)
    {
        var userIdIsValid = UserId.TryCreate(userId, out var userIdResult);
        if (!userIdIsValid)
        {
            return AppointmentsModuleDomainErrors.AppointmentErrors.AttendeeEmptyId;
        }
        
        var appointmentIdIsValid = AppointmentId.TryCreate(appointmentId, out var appointmentIdResult);
        if (!appointmentIdIsValid)
        {
            return AppointmentsModuleDomainErrors.AppointmentErrors.AppointmentEmptyId;
        }

        var attendee = new Attendee(userIdResult, appointmentIdResult);
        return attendee;
    }
}