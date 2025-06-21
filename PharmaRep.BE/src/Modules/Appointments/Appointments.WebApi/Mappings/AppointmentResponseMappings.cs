using Appointments.Application.Dtos;
using Appointments.Application.Features.Appointment.GetAll;
using Appointments.WebApi.Responses;
using Shared.WebApi.Responses;

namespace Appointments.WebApi.Mappings;

public static class AppointmentResponseMappings
{
    public static CreateAppointmentResponse ToResponse(Guid appointmentId) => new(appointmentId);
    
    internal static PaginatedResponse<AppointmentDto> ToGetAllAppointmentsResponse(AppointmentsPaginatedResult paginatedAppointments)
        => new(pageNumber: paginatedAppointments.PageNumber,
            pageSize: paginatedAppointments.PageSize,
            total: paginatedAppointments.Total,
            items: paginatedAppointments.Items.ToList());

    internal static GetAppointmentResponse ToGetAppointmentResponse(AppointmentDto appointment) => new(Id: appointment.Id,
        Start: appointment.Start,
        End: appointment.End,
        Address: appointment.Address,
        Organizer: appointment.Organizer,
        Attendees: appointment.Attendees);
}