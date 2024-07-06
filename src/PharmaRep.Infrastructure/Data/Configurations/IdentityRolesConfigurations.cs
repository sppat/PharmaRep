using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PharmaRep.Application.Constants;

namespace PharmaRep.Infrastructure.Data.Configurations;

public class IdentityRolesConfigurations() : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(GetRoles());
    }

    private static IEnumerable<IdentityRole> GetRoles() => 
    [
        new IdentityRole
        {
            Id = "13c90e8a-b6df-403d-a0f3-be6c2576a0fc",
            Name = RoleConstants.Admin,
            ConcurrencyStamp = "c4762ffe-86e7-44b7-8cea-e459f06e13fd"
        },
        new IdentityRole
        {
            Id = "fc5fb185-e1e6-42b3-b51e-8dc6eb22196e",
            Name = RoleConstants.Midwife,
            ConcurrencyStamp = "b0711fc0-76fe-455b-80e4-ffe306b19af3"
        },
        new IdentityRole
        {
            Id = "ee6f7539-69cd-46e7-9068-50b3ffc17db4",
            Name = RoleConstants.PharmaceuticalRepresentative,
            ConcurrencyStamp = "e3fcc74b-2fa8-4f2b-a2da-9bf8b7ef7748"
        }
    ];
}
