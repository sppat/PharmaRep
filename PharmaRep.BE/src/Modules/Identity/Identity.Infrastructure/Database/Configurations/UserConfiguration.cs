using Identity.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	private const int MaxNameLength = 50;
	private const int MaxEmailLength = 100;

	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.Property(u => u.FirstName).HasMaxLength(MaxNameLength);
		builder.Property(u => u.LastName).HasMaxLength(MaxNameLength);
		builder.Property(u => u.Email).HasMaxLength(MaxEmailLength);
	}
}
