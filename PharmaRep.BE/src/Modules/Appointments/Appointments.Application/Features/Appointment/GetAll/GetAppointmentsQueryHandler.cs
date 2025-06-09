using Appointments.Application.Abstractions;
using Appointments.Application.Mappings;
using Identity.Public.Features;
using Microsoft.AspNetCore.Http.Internal;
using Shared.Application.Mediator;
using Shared.Application.Results;

namespace Appointments.Application.Features.Appointment.GetAll;

public class GetAppointmentsQueryHandler(IDispatcher dispatcher, 
    IAppointmentRepository appointmentRepository) : IRequestHandler<GetAppointmentsQuery, Result<AppointmentsPaginatedResult>>
{
    public async Task<Result<AppointmentsPaginatedResult>> HandleAsync(GetAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var appointments = (await appointmentRepository.GetAllAsync(userId: request.UserId,
            from: request.From,
            to: request.To,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize,
            cancellationToken: cancellationToken)).ToList();

        var organizerIds = appointments.Select(appointment => appointment.CreatedBy.Value).ToList();
        var attendeeIds = appointments.SelectMany(appointment => appointment.Attendees)
            .Select(attendee => attendee.UserId.Value)
            .ToList();

        var userIdsToRequest = new HashSet<Guid>(organizerIds);
        attendeeIds.ForEach(attendeeId => userIdsToRequest.Add(attendeeId));
        
        var getUsersQuery = new GetUsersBasicInfoQuery(userIdsToRequest);
        var usersResult = await dispatcher.SendAsync(getUsersQuery, cancellationToken);

        var appointmentDtos = appointments.Select(appointment =>
        {
            var organizer = usersResult.Value.SingleOrDefault(user => user.Id == appointment.CreatedBy.Value);
            var attendees = appointment.Attendees
                .Select(attendee => usersResult.Value.SingleOrDefault(u => u.Id == attendee.UserId.Value))
                .ToList();
            
            return appointment.ToDto(organizer, attendees);
        }).ToList();
        
        var totalAppointments = await appointmentRepository.CountAsync(cancellationToken);
        var paginatedResult = new AppointmentsPaginatedResult(PageNumber: request.PageNumber, 
            PageSize: request.PageSize,
            Total: totalAppointments,
            Items: appointmentDtos);
        
        return Result<AppointmentsPaginatedResult>.Success(paginatedResult);
    }
}