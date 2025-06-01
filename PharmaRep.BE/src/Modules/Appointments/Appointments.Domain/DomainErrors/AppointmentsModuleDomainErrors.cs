namespace Appointments.Domain.DomainErrors;

public static class AppointmentsModuleDomainErrors
{
    public static class AppointmentErrors
    {
        public const string InvalidDate = "Appointment's date is either empty or end date must be after start date";
        public const string InvalidAddress = "The address of the appointment or the zip code is empty";
        public const string EmptyOrganizerId = "The organizer id cannot be empty";
        public const string AttendeeEmptyId = "Attendee id cannot be empty";
        public const string AppointmentEmptyId = "Appointment id cannot be empty";
    }
}