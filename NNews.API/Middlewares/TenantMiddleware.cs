namespace NNews.API.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Read X-Tenant-Id header and store in HttpContext.Items
            // This runs BEFORE authentication, so for non-authenticated endpoints
            // the TenantId will be resolved from the header.
            // For authenticated endpoints, TenantContext will prefer the JWT claim.
            if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdHeader))
            {
                var tenantId = tenantIdHeader.ToString();
                if (!string.IsNullOrWhiteSpace(tenantId))
                {
                    context.Items["TenantId"] = tenantId;
                }
            }

            await _next(context);
        }
    }
}
