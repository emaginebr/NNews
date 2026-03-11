using Microsoft.EntityFrameworkCore;

namespace NNews.Application.Interfaces
{
    public interface ITenantDbContextFactory<TContext> where TContext : DbContext
    {
        TContext CreateDbContext();
    }
}
