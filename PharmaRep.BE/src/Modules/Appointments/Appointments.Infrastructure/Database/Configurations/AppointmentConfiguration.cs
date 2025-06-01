using Appointments.Domain.Entities;
using Appointments.Infrastructure.Database.Configurations.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointments.Infrastructure.Database.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(appointment => appointment.Id);
        
        builder.Property(appointment => appointment.Id)
            .HasConversion(new AppointmentIdConverter());

        builder.ComplexProperty(appointment => appointment.Date);
        builder.ComplexProperty(appointment => appointment.Address);

        builder.HasMany(appointment => appointment.Attendees)
            .WithMany();
    }
}