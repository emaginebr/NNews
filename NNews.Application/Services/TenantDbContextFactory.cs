using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NNews.Application.Interfaces;
using NNews.Infra.Context;

namespace NNews.Application.Services
{
    public class TenantDbContextFactory : ITenantDbContextFactory<NNewsContext>
    {
        private readonly ITenantContext _tenantContext;
        private readonly IConfiguration _configuration;

        public TenantDbContextFactory(ITenantContext tenantContext, IConfiguration configuration)
        {
            _tenantContext = tenantContext ?? throw new ArgumentNullException(nameof(tenantContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public NNewsContext CreateDbContext()
        {
            var tenantId = _tenantContext.TenantId;

            var connectionString = _configuration[$"Tenants:{tenantId}:ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException($"ConnectionString not found for tenant '{tenantId}'. Ensure 'Tenants:{tenantId}:ConnectionString' is configured in appsettings.json.");

            var optionsBuilder = new DbContextOptionsBuilder<NNewsContext>();
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseNpgsql(connectionString)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();

            return new NNewsContext(optionsBuilder.Options);
        }
    }
}
