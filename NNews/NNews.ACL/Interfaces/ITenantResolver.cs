namespace NNews.ACL.Interfaces
{
    public interface ITenantResolver
    {
        string TenantId { get; }
        string ConnectionString { get; }
        string JwtSecret { get; }
    }
}
