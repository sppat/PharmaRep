namespace Appointments.WebApi.Requests;

public record CreateAppointmentRequest(DateTime StartDate,
    DateTime EndDate,
    string Street,
    ushort Number,
    uint ZipCode,
    Guid OrganizerId,
    IEnumerable<Guid> AttendeeIds);
