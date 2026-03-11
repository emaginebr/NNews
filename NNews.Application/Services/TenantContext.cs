using Microsoft.AspNetCore.Http;
using NNews.Application.Interfaces;
using System.Security.Claims;

namespace NNews.Application.Services
{
    public class TenantContext : ITenantContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string TenantId
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                    throw new InvalidOperationException("HttpContext is not available.");

                // Authenticated: resolve from JWT claim
                var tenantClaim = httpContext.User?.FindFirst("tenant_id");
                if (tenantClaim != null && !string.IsNullOrEmpty(tenantClaim.Value))
                    return tenantClaim.Value;

                // Non-authenticated: resolve from HttpContext.Items (set by TenantMiddleware)
                if (httpContext.Items.TryGetValue("TenantId", out var tenantIdObj) && tenantIdObj is string tenantId && !string.IsNullOrEmpty(tenantId))
                    return tenantId;

                throw new InvalidOperationException("TenantId could not be resolved from JWT claims or HTTP headers.");
            }
        }
    }
}
