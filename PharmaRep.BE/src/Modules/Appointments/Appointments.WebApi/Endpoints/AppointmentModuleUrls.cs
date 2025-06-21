namespace Appointments.WebApi.Endpoints;

public static class AppointmentModuleUrls
{
    private const string ModuleBaseUrl = "api";

    public static class Appointment
    {
        private const string AppointmentBaseUrl = $"{ModuleBaseUrl}/appointments";

        public const string Create = AppointmentBaseUrl;
        public const string GetAll = AppointmentBaseUrl;
        public const string GetAppointment = $"{AppointmentBaseUrl}/{{id:guid}}";
    }
}