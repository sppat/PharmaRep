using Appointments.Domain.DomainErrors;

using Shared.Domain.Exceptions;

namespace Appointments.Domain.Exceptions.Appointment;

public class EmptyAppointmentIdException(string param) : DomainExceptionBase(AppointmentsModuleDomainErrors.AppointmentErrors.EmptyId, param);
