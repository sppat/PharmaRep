namespace Appointments.Domain.DomainErrors;

public static class AppointmentsModuleDomainErrors
{
    public static class AppointmentErrors
    {
        public const string PastDate = "The date of the appointment cannot be in the past";
        public const string InvalidEndDate = "The end date of the appointment must be after the start date";
        public const string EmptyAddress = "The address of the appointment cannot be empty";
    }
}