namespace Appointments.WebApi.Requests;

public record CreateAppointmentRequest(DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string Street,
    ushort Number,
    uint ZipCode,
    Guid OrganizerId,
    IEnumerable<Guid> AttendeeIds);
