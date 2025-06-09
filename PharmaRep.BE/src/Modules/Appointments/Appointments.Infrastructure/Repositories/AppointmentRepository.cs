using Appointments.Application.Abstractions;
using Appointments.Application.Dtos;
using Appointments.Domain.Entities;
using Appointments.Infrastructure.Database;
using Identity.Public.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Repositories;

public class AppointmentRepository(PharmaRepAppointmentsDbContext dbContext) : IAppointmentRepository
{
    public async Task<int> CountAsync(CancellationToken cancellationToken) 
        => await dbContext.Appointments.CountAsync(cancellationToken);
    

    public async Task AddAsync(Appointment appointment, CancellationToken cancellationToken)
        => await dbContext.Appointments.AddAsync(appointment, cancellationToken);

    public async Task<ICollection<Appointment>> GetAllAsync(Guid? userId = null,
        DateTimeOffset? from = null,
        DateTimeOffset? to = null,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var appointments = dbContext.Appointments
            .Include(appointment => appointment.Attendees)
            .OrderBy(appointment => appointment.Id)
            .AsNoTracking();
        
        if (userId.HasValue)
        {
            var users = appointments.SelectMany(appointment => appointment.Attendees)
                .Select(attendee => attendee.UserId.Value);

            appointments = appointments.Where(appointment => appointment.CreatedBy.Value == userId.Value)
                .Where(appointment => users.Contains(userId.Value));
        }

        if (from.HasValue)
        {
            appointments = appointments.Where(appointment => appointment.Date.StartDate >= from.Value);
        }
        
        if (to.HasValue)
        {
            appointments = appointments.Where(appointment => appointment.Date.EndDate <= to.Value);
        }
        
        var queryResult = appointments.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
        
        return await queryResult.ToListAsync(cancellationToken);
    }
}