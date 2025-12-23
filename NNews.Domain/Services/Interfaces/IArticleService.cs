using NNews.Dtos;

namespace NNews.Domain.Services.Interfaces
{
    public interface IArticleService
    {
        IList<ArticleInfo> ListAll(long? categoryId);
        IList<ArticleInfo> FilterByRolesAndParent(IList<string>? roles, long? parentId);
        ArticleInfo GetById(int articleId);
        ArticleInfo Insert(ArticleInfo article);
        ArticleInfo Update(ArticleInfo article);
    }
}
