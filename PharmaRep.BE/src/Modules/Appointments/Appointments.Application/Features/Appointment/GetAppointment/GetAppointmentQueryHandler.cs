using Appointments.Application.Abstractions;
using Appointments.Application.Dtos;
using Appointments.Application.Mappings;
using Appointments.Domain.DomainErrors;
using Appointments.Domain.ValueObjects;
using Identity.Public.Features.GetUsersBasicInfo;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Appointments.Application.Features.Appointment.GetAppointment;

public class GetAppointmentQueryHandler(IDispatcher dispatcher, IAppointmentRepository appointmentRepository) : IRequestHandler<GetAppointmentQuery, Result<AppointmentDto>>
{
    public async Task<Result<AppointmentDto>> HandleAsync(GetAppointmentQuery request, CancellationToken cancellationToken)
    {
        var validAppointmentId = AppointmentId.TryCreate(request.Id, out var appointmentId);
        if (!validAppointmentId)
        {
            return Result<AppointmentDto>.Failure([AppointmentsModuleDomainErrors.AppointmentErrors.AppointmentEmptyId], ResultType.ValidationError);
        }
        
        var appointment = await appointmentRepository.GetByIdAsync(id: appointmentId, asNoTracking: true, cancellationToken: cancellationToken);
        if (appointment is null)
        {
            return Result<AppointmentDto>.Failure([AppointmentsModuleDomainErrors.AppointmentErrors.AppointmentNotFound], ResultType.NotFound);
        }

        var getUsersQuery = new GetUsersBasicInfoQuery(UsersId: appointment.GetOrganizerAndAttendeesId());
        var getUsersResult = await dispatcher.SendAsync(getUsersQuery, cancellationToken);
        var usersInfo = getUsersResult.Value.ToList();
        
        var organizer = usersInfo.SingleOrDefault(userInfo => userInfo.Id == appointment.CreatedBy.Value);
        var attendees = appointment.Attendees
            .Select(attendee => usersInfo.SingleOrDefault(userInfo => userInfo.Id == attendee.UserId.Value));
        
        var dto = appointment.ToDto(organizer, attendees);
        
        return Result<AppointmentDto>.Success(dto);
    }
}