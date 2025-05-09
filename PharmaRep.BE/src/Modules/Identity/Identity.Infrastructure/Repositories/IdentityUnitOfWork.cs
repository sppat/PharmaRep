using Identity.Application.Interfaces;
using Identity.Infrastructure.Database;

namespace Identity.Infrastructure.Repositories;

public class IdentityUnitOfWork(PharmaRepIdentityDbContext dbContext) : IIdentityUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => await dbContext.SaveChangesAsync(cancellationToken);
}