using Appointments.Application.Dtos;
using Appointments.Domain.Entities;
using Identity.Public.Contracts;

namespace Appointments.Application.Abstractions;

public interface IAppointmentRepository
{
    Task<int> CountAsync(CancellationToken cancellationToken);
    Task AddAsync(Appointment appointment, CancellationToken cancellationToken);
    Task<IEnumerable<Appointment>> GetAllAsync(Guid? userId = null,
        DateTimeOffset? from = null,
        DateTimeOffset? to = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
}