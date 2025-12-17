namespace Appointments.Domain.DomainErrors;

public static class AppointmentsModuleDomainErrors
{
    public static class AppointmentErrors
    {
        public const string InvalidValue = "Value cannot be empty";
        public const string EmptyId = "Id cannot be empty";
        
        public const string StartDateAfterEndDate = "The start date and time cannot be equal or after the end date";
        public const string AppointmentNotFound = "Appointment with the given id doesn't exist";
    }
}