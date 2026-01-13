using Appointments.Domain.DomainErrors;

using Shared.Domain.Exceptions;

namespace Appointments.Domain.Exceptions.Appointment;

public class AppointmentStartDateException() : DomainExceptionBase(AppointmentsModuleDomainErrors.AppointmentErrors.StartDateAfterEndDate);
