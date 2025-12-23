using NNews.Domain.Entities.Interfaces;

namespace NNews.Domain.Entities
{
    public class CategoryModel : ICategoryModel
    {
        public long CategoryId { get; private set; }
        public long? ParentId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string Title { get; private set; }
        public int ArticleCount { get; private set; }

        private CategoryModel()
        {
            Title = string.Empty;
            ArticleCount = 0;
        }

        public CategoryModel(string title, long? parentId = null) : this()
        {
            SetTitle(title);
            ParentId = parentId;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public static CategoryModel Create(string title, long? parentId = null)
        {
            return new CategoryModel(title, parentId);
        }

        public static CategoryModel Reconstruct(long categoryId, string title, long? parentId,
            DateTime createdAt, DateTime updatedAt)
        {
            var category = new CategoryModel
            {
                CategoryId = categoryId,
                ParentId = parentId,
                Title = title,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt
            };

            return category;
        }

        public static CategoryModel Reconstruct(long categoryId, string title, long? parentId,
            DateTime createdAt, DateTime updatedAt, int articleCount)
        {
            var category = new CategoryModel
            {
                CategoryId = categoryId,
                ParentId = parentId,
                Title = title,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt,
                ArticleCount = articleCount
            };

            return category;
        }

        public void UpdateTitle(string title)
        {
            SetTitle(title);
            UpdateTimestamp();
        }

        public void UpdateParentId(long? parentId)
        {
            ParentId = parentId;
            UpdateTimestamp();
        }

        public void Update(string title, long? parentId)
        {
            SetTitle(title);
            ParentId = parentId;
            UpdateTimestamp();
        }

        private void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));

            if (title.Length > 240)
                throw new ArgumentException("Title cannot exceed 240 characters.", nameof(title));

            Title = title.Trim();
        }

        private void UpdateTimestamp()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not CategoryModel other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (CategoryId == 0 || other.CategoryId == 0)
                return false;

            return CategoryId == other.CategoryId;
        }

        public override int GetHashCode()
        {
            return CategoryId.GetHashCode();
        }

        public override string ToString()
        {
            return $"Category: {Title} (ParentId: {ParentId?.ToString() ?? "null"})";
        }
    }
}
