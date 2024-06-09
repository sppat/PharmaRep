using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PharmaRep.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext(options)
{

}
