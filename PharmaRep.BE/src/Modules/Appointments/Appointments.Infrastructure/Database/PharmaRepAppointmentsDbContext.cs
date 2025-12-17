using Appointments.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Constants;
using Shared.Infrastructure.Converters;

namespace Appointments.Infrastructure.Database;

public class PharmaRepAppointmentsDbContext : DbContext
{
    public PharmaRepAppointmentsDbContext(DbContextOptions<PharmaRepAppointmentsDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(EfConstants.Schemas.Appointments);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PharmaRepAppointmentsDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Handle regular properties
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTimeOffset))
                {
                    property.SetValueConverter(new UtcConverter());
                }
                else if (property.ClrType == typeof(DateTimeOffset?))
                {
                    property.SetValueConverter(new NullableUtcConverter());
                }
            }

            // Handle complex (value object) properties
            foreach (var complexProperty in entityType.GetComplexProperties())
            {
                foreach (var property in complexProperty.ComplexType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTimeOffset))
                    {
                        property.SetValueConverter(new UtcConverter());
                    }
                    else if (property.ClrType == typeof(DateTimeOffset?))
                    {
                        property.SetValueConverter(new NullableUtcConverter());
                    }
                }
            }
        }

        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Attendee> Attendees { get; set; }
}