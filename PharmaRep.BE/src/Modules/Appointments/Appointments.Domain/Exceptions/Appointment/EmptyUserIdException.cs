using Appointments.Domain.DomainErrors;

using Shared.Domain.Exceptions;

namespace Appointments.Domain.Exceptions.Appointment;

public class EmptyUserIdException(string param) : DomainExceptionBase(AppointmentsModuleDomainErrors.AppointmentErrors.EmptyId, param);
