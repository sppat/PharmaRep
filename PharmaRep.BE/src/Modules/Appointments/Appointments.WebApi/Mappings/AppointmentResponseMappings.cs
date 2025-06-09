using Appointments.Application.Dtos;
using Appointments.Application.Features.Appointment.GetAll;
using Appointments.WebApi.Responses;
using Identity.Application.Features.User.GetAll;
using Shared.WebApi.Responses;

namespace Appointments.WebApi.Mappings;

public static class AppointmentResponseMappings
{
    public static CreateAppointmentResponse ToResponse(Guid appointmentId) => new(appointmentId);
    
    internal static PaginatedResponse<AppointmentDto> ToGetAllUsersResponse(AppointmentsPaginatedResult paginatedAppointments)
        => new(pageNumber: paginatedAppointments.PageNumber,
            pageSize: paginatedAppointments.PageSize,
            total: paginatedAppointments.Total,
            items: paginatedAppointments.Items.ToList());
}