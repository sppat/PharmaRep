using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Constants;

namespace Identity.Infrastructure.Database;

public class PharmaRepIdentityDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public PharmaRepIdentityDbContext(DbContextOptions<PharmaRepIdentityDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.HasDefaultSchema(EfConstants.Schemas.Identity);
        builder.ApplyConfigurationsFromAssembly(typeof(PharmaRepIdentityDbContext).Assembly);
    }
}