namespace Appointments.Application.Abstractions;

public interface IAppointmentRepository
{
    Task AddAsync(Domain.Entities.Appointment appointment, CancellationToken cancellationToken);
}