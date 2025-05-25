using Appointments.Domain.DomainErrors;

namespace Appointments.Domain.Exceptions;

public class AppointmentEmptyAddressException() : Exception(AppointmentsModuleDomainErrors.AppointmentErrors.EmptyAddress);