using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaRep.Domain;

namespace PharmaRep.Infrastructure.Data.Configurations;

public class RolesConfigurations() : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData();
    }

    private IEnumerable<Role> GetRoles() => 
    [
        new Role("Midwife"),
        new Role("PharmaceuticalRepresentative")
    ];
}
