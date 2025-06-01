using Appointments.Domain.Entities;
using Appointments.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Constants;

namespace Appointments.Infrastructure.Database;

public class PharmaRepAppointmentsDbContext : DbContext
{
    public PharmaRepAppointmentsDbContext(DbContextOptions<PharmaRepAppointmentsDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(EfConstants.Schemas.Appointments);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PharmaRepAppointmentsDbContext).Assembly);
    }
    
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Attendee> Attendees { get; set; }
}