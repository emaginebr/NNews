using NNews.Domain.Entities.Interfaces;

namespace NNews.Domain.Entities
{
    public class RoleModel : IRoleModel
    {
        public string Slug { get; private set; }
        public string Name { get; private set; }

        private RoleModel()
        {
            Slug = string.Empty;
            Name = string.Empty;
        }

        public RoleModel(string slug, string name)
        {
            if (string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("Slug cannot be null or empty.", nameof(slug));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));

            Slug = slug.Trim();
            Name = name.Trim();
        }

        public static RoleModel Create(string slug, string name)
        {
            return new RoleModel(slug, name);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not RoleModel other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Slug.Equals(other.Slug, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Slug.ToLowerInvariant().GetHashCode();
        }

        public override string ToString()
        {
            return $"Role: {Name} ({Slug})";
        }
    }
}
