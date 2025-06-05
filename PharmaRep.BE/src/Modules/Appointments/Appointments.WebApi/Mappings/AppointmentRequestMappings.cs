using Appointments.Application.Features.Appointment.Create;
using Appointments.WebApi.Requests;

namespace Appointments.WebApi.Mappings;

public static class AppointmentRequestMappings
{
    public static CreateAppointmentCommand ToCommand(this CreateAppointmentRequest request) => new(StartDate: request.StartDate,
        EndDate: request.EndDate,
        Street: request.Street,
        Number: request.Number,
        ZipCode: request.ZipCode,
        OrganizerId: request.OrganizerId,
        AttendeeIds: request.AttendeeIds);
}