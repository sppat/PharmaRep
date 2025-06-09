namespace Appointments.WebApi.Requests;

public record GetAppointmentsRequest(Guid? UserId = null,
    DateTimeOffset? From = null,
    DateTimeOffset? To = null,
    int PageNumber = 1,
    int PageSize = 10);