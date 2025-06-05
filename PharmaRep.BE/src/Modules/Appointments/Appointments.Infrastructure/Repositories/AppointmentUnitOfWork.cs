using Appointments.Application.Abstractions;
using Appointments.Infrastructure.Database;

namespace Appointments.Infrastructure.Repositories;

public class AppointmentUnitOfWork(PharmaRepAppointmentsDbContext dbContext) : IAppointmentUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken) => await dbContext.SaveChangesAsync(cancellationToken);
}