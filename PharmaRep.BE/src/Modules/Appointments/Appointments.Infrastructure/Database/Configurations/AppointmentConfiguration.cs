using Appointments.Domain.Entities;
using Appointments.Infrastructure.Database.Configurations.Converters;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Shared.Infrastructure.Constants;

namespace Appointments.Infrastructure.Database.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
	public void Configure(EntityTypeBuilder<Appointment> builder)
	{
		builder.HasKey(appointment => appointment.Id);
		builder.Property(appointment => appointment.Id).HasConversion(new AppointmentIdConverter());

		builder.Property(appointment => appointment.StartDate).IsRequired().HasConversion(new AppointmentDateConverter());
		builder.Property(appointment => appointment.EndDate).IsRequired().HasConversion(new AppointmentDateConverter());

		builder.OwnsOne(appointment => appointment.Address, addressBuilder =>
		{
			addressBuilder.ToTable(EfConstants.OwnedTables.Appointment.Address);

			addressBuilder.Property(EfConstants.ShadowProperties.AppointmentAddress.AppointmentId);
			addressBuilder.HasKey(EfConstants.ShadowProperties.AppointmentAddress.AppointmentId);

			addressBuilder.Property(address => address.Street).HasConversion(new StreetConverter());
			addressBuilder.Property(address => address.Street).IsRequired().HasMaxLength(200);

			addressBuilder.Property(address => address.ZipCode).HasConversion(new ZipCodeConverter());

			addressBuilder.HasIndex(EfConstants.ShadowProperties.AppointmentAddress.AppointmentId).IsUnique();
		});

		builder.Property(appointment => appointment.CreatedBy).IsRequired().HasConversion(new UserIdConverter());
		builder.Property(appointment => appointment.UpdatedBy).HasConversion(new UserIdConverter());

		builder.HasMany(appointment => appointment.Attendees).WithOne().HasForeignKey(attendee => attendee.AppointmentId).OnDelete(DeleteBehavior.Cascade);

		builder.HasIndex(appointment => appointment.Id).IsUnique();
		builder.HasIndex(appointment => appointment.CreatedBy);
	}
}
