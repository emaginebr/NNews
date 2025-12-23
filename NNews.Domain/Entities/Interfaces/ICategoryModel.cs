namespace NNews.Domain.Entities.Interfaces
{
    public interface ICategoryModel
    {
        long CategoryId { get; }
        long? ParentId { get; }
        DateTime CreatedAt { get; }
        DateTime UpdatedAt { get; }
        string Title { get; }
        int ArticleCount { get; }

        void UpdateTitle(string title);
        void UpdateParentId(long? parentId);
        void Update(string title, long? parentId);
    }
}
