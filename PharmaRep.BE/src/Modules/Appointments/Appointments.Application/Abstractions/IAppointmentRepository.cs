﻿using Appointments.Domain.Entities;
using Appointments.Domain.ValueObjects;

namespace Appointments.Application.Abstractions;

public interface IAppointmentRepository
{
    Task<int> CountAsync(CancellationToken cancellationToken);
    Task AddAsync(Appointment appointment, CancellationToken cancellationToken);
    Task<ICollection<Appointment>> GetAllAsync(Guid? userId = null,
        DateTimeOffset? from = null,
        DateTimeOffset? to = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
    Task<Appointment> GetByIdAsync(AppointmentId id, bool asNoTracking = false, CancellationToken cancellationToken = default);
}