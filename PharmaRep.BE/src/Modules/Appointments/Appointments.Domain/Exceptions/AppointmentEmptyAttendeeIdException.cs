using Appointments.Domain.DomainErrors;

namespace Appointments.Domain.Exceptions;

public class AppointmentEmptyAttendeeIdException() : Exception(AppointmentsModuleDomainErrors.AppointmentErrors.AttendeeEmptyId);