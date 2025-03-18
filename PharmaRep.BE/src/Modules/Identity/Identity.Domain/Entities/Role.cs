using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    public ICollection<UserRole> UserRoles { get; private set; }
    
    public static Role Admin => new()
    {
        Id = Guid.Parse("02a12708-fc75-4b6c-9111-10b5f7eadf56"),
        Name = "Admin",
        NormalizedName = "ADMIN"
    };

    public static Role Doctor => new()
    {
        Id = Guid.Parse("1f1a3cf9-c300-4821-9b5a-e15f8bbdabd0"),
        Name = "Doctor",
        NormalizedName = "DOCTOR"
    };

    public static Role Midwife => new()
    {
        Id = Guid.Parse("8ab04aba-48f0-4b3a-bbae-5601f8da9f66"),
        Name = "Midwife",
        NormalizedName = "MIDWIFE"
    };

    public static Role MedicalRepresentative => new()
    {
        Id = Guid.Parse("06d4065d-4544-4511-9f32-d054a13afe85"),
        Name = "Medical Representative",
        NormalizedName = "MEDICAL REPRESENTATIVE"
    };

    public static IEnumerable<Role> All =>
    [
        Admin,
        Doctor,
        Midwife,
        MedicalRepresentative
    ];
}