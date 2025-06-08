using Appointments.Application.Abstractions;
using Appointments.Domain.Entities;
using Appointments.Infrastructure.Database;

namespace Appointments.Infrastructure.Repositories;

public class AppointmentRepository(PharmaRepAppointmentsDbContext dbContext) : IAppointmentRepository
{
    public async Task AddAsync(Appointment appointment, CancellationToken cancellationToken)
        => await dbContext.Appointments.AddAsync(appointment, cancellationToken);
}