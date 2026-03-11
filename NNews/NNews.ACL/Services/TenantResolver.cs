using Microsoft.Extensions.Configuration;
using NNews.ACL.Interfaces;

namespace NNews.ACL.Services
{
    public class TenantResolver : ITenantResolver
    {
        private readonly IConfiguration _configuration;
        private readonly string _tenantId;

        public TenantResolver(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _tenantId = _configuration["Tenant:DefaultTenantId"]
                ?? throw new InvalidOperationException("Tenant:DefaultTenantId is not configured in appsettings.json.");
        }

        public string TenantId => _tenantId;

        public string ConnectionString
        {
            get
            {
                var connectionString = _configuration[$"Tenants:{_tenantId}:ConnectionString"];
                if (string.IsNullOrEmpty(connectionString))
                    throw new InvalidOperationException($"ConnectionString not found for tenant '{_tenantId}'. Ensure 'Tenants:{_tenantId}:ConnectionString' is configured in appsettings.json.");

                return connectionString;
            }
        }

        public string JwtSecret
        {
            get
            {
                var jwtSecret = _configuration[$"Tenants:{_tenantId}:JwtSecret"];
                if (string.IsNullOrEmpty(jwtSecret))
                    throw new InvalidOperationException($"JwtSecret not found for tenant '{_tenantId}'. Ensure 'Tenants:{_tenantId}:JwtSecret' is configured in appsettings.json.");

                return jwtSecret;
            }
        }
    }
}
