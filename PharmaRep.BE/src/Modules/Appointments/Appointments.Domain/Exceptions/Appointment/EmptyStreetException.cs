using Appointments.Domain.DomainErrors;

using Shared.Domain.Exceptions;

namespace Appointments.Domain.Exceptions.Appointment;

public class EmptyStreetException(string param) : DomainExceptionBase(AppointmentsModuleDomainErrors.AppointmentErrors.InvalidValue, param);
