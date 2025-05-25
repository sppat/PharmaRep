namespace Appointments.Domain.Exceptions;

public class AppointmentDateException(string message) : Exception(message) { }