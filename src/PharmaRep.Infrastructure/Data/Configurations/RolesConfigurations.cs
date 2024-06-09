using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaRep.Application.Constants;
using PharmaRep.Domain;

namespace PharmaRep.Infrastructure.Data.Configurations;

public class RolesConfigurations() : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(GetRoles());
    }

    private static IEnumerable<Role> GetRoles() => 
    [
        new Role(name: RoleConstants.Midwife),
        new Role(name: RoleConstants.PharmaceuticalRepresentative)
    ];
}
