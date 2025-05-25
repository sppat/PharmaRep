namespace Appointments.Domain.DomainErrors;

public static class AppointmentsModuleDomainErrors
{
    public static class AppointmentErrors
    {
        public const string EmptyDate = "Date of the appointment cannot be empty";
        public const string InvalidEndDate = "The end date of the appointment must be after the start date";
        public const string EmptyAddress = "The address of the appointment cannot be empty";
        public const string EmptyOrganizerId = "The organizer id cannot be empty";
        public const string AttendeeEmptyId = "Attendee id cannot be empty";
    }
}