using Microsoft.Extensions.Configuration;
using NAuth.ACL.Interfaces;

namespace NNews.Application.Services
{
    public class NAuthTenantSecretProvider : ITenantSecretProvider
    {
        private readonly IConfiguration _configuration;

        public NAuthTenantSecretProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string? GetJwtSecret(string tenantId)
        {
            return _configuration[$"Tenants:{tenantId}:JwtSecret"];
        }
    }
}
