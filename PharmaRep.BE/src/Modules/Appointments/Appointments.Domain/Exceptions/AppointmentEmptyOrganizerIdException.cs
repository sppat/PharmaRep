using Appointments.Domain.DomainErrors;

namespace Appointments.Domain.Exceptions;

public class AppointmentEmptyOrganizerIdException() : Exception(AppointmentsModuleDomainErrors.AppointmentErrors.EmptyOrganizerId);