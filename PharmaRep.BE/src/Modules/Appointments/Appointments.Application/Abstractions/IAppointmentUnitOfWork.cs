namespace Appointments.Application.Abstractions;

public interface IAppointmentUnitOfWork
{
	Task SaveChangesAsync(CancellationToken cancellationToken);
}
