using Appointments.Application.Abstractions;
using Appointments.Application.Mappings;
using Identity.Public.Features.GetUsersBasicInfo;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Appointments.Application.Features.Appointment.GetAll;

public class GetAppointmentsQueryHandler(IDispatcher dispatcher, 
    IAppointmentRepository appointmentRepository) : IRequestHandler<GetAppointmentsQuery, Result<AppointmentsPaginatedResult>>
{
    public async Task<Result<AppointmentsPaginatedResult>> HandleAsync(GetAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var appointments = await appointmentRepository.GetAllAsync(userId: request.UserId,
            from: request.From,
            to: request.To,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            cancellationToken: cancellationToken);

        var organizerIds = appointments.Select(appointment => appointment.CreatedBy.Value);
        var attendeeIds = appointments.SelectMany(appointment => appointment.Attendees)
            .Select(attendee => attendee.UserId.Value);

        var userIdsToRequest = new HashSet<Guid>(organizerIds);
        foreach (var attendeeId in attendeeIds)
        {
            userIdsToRequest.Add(attendeeId);
        }
        
        var getUsersQuery = new GetUsersBasicInfoQuery(userIdsToRequest);
        var getUsersResult = await dispatcher.SendAsync(getUsersQuery, cancellationToken);
        var usersInfo = getUsersResult.Value.ToList();

        var appointmentDtos = appointments.Select(appointment =>
        {
            var organizer = usersInfo.SingleOrDefault(user => user.Id == appointment.CreatedBy.Value);
            var attendees = appointment.Attendees
                .Select(attendee => usersInfo.SingleOrDefault(u => u.Id == attendee.UserId.Value))
                .ToList();
            
            return appointment.ToDto(organizer, attendees);
        });
        
        var totalAppointments = await appointmentRepository.CountAsync(cancellationToken);
        var paginatedResult = new AppointmentsPaginatedResult(PageNumber: request.PageNumber, 
            PageSize: request.PageSize,
            Total: totalAppointments,
            Items: appointmentDtos);
        
        return Result<AppointmentsPaginatedResult>.Success(paginatedResult);
    }
}