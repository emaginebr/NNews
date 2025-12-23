namespace NNews.Domain.Entities.Interfaces
{
    public interface ITagModel
    {
        long TagId { get; }
        string Slug { get; }
        string Title { get; }
        int ArticleCount { get; }

        void UpdateTitle(string title);
        void UpdateSlug(string slug);
        void Update(string title, string slug);
    }
}
