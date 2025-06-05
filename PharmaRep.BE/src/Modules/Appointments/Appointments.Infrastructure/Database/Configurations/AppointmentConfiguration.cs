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
        builder.Property(appointment => appointment.Id).HasConversion(new AppointmentIdConverter());
        
        builder.ComplexProperty(appointment => appointment.Date).IsRequired();
        builder.ComplexProperty(appointment => appointment.Address).IsRequired();
        
        builder.Property(appointment => appointment.CreatedBy).IsRequired().HasConversion(new UserIdConverter());
        builder.Property(appointment => appointment.UpdatedBy).HasConversion(new UserIdConverter());

        builder.HasMany(appointment => appointment.Attendees).WithOne().HasForeignKey(attendee => attendee.AppointmentId).OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(appointment => appointment.Id).IsUnique();
        builder.HasIndex(appointment => appointment.CreatedBy);
    }
}