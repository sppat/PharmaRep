using Appointments.Domain.Entities;
using Appointments.Domain.ValueObjects;
using Appointments.Infrastructure.Database.Configurations.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointments.Infrastructure.Database.Configurations;

public class AttendeeConfiguration : IEntityTypeConfiguration<Attendee>
{
    public void Configure(EntityTypeBuilder<Attendee> builder)
    {
        builder.HasKey(attendee => new { attendee.UserId, attendee.AppointmentId } );
        
        builder.Property(attendee => attendee.UserId)
            .HasConversion(new UserIdConverter());
        
        builder.Property(attendee => attendee.AppointmentId)
            .HasConversion(new AppointmentIdConverter());
    }
}