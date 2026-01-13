using Appointments.Domain.DomainErrors;

using Shared.Domain.Exceptions;

namespace Appointments.Domain.Exceptions.Appointment;

public class InvalidZipCodeException(string param) : DomainExceptionBase(AppointmentsModuleDomainErrors.AppointmentErrors.InvalidValue, param);
