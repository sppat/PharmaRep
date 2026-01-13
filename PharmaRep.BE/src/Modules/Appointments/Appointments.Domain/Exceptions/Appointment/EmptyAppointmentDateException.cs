using Appointments.Domain.DomainErrors;

using Shared.Domain.Exceptions;

namespace Appointments.Domain.Exceptions.Appointment;

public class EmptyAppointmentDateException(string param) : DomainExceptionBase(AppointmentsModuleDomainErrors.AppointmentErrors.InvalidValue, param);
