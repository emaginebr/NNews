using Microsoft.Extensions.Configuration;

namespace NNews.ACL.Handlers
{
    public class TenantHeaderHandler : DelegatingHandler
    {
        private readonly IConfiguration _configuration;

        public TenantHeaderHandler(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tenantId = _configuration["Tenant:DefaultTenantId"];
            if (!string.IsNullOrEmpty(tenantId))
                request.Headers.TryAddWithoutValidation("X-Tenant-Id", tenantId);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
