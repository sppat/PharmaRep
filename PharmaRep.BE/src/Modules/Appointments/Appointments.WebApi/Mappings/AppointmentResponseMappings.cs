using Appointments.WebApi.Responses;

namespace Appointments.WebApi.Mappings;

public static class AppointmentResponseMappings
{
    public static CreateAppointmentResponse ToResponse(this Guid appointmentId) => new(appointmentId);
}