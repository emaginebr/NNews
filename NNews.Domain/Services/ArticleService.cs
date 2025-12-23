using AutoMapper;
using NNews.Domain.Entities;
using NNews.Domain.Entities.Interfaces;
using NNews.Domain.Services.Interfaces;
using NNews.Dtos;
using NNews.Infra.Interfaces.Repository;

namespace NNews.Domain.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository<IArticleModel> _articleRepository;
        private readonly IMapper _mapper;

        public ArticleService(IArticleRepository<IArticleModel> articleRepository, IMapper mapper)
        {
            _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IList<ArticleInfo> ListAll(long? categoryId)
        {
            var articles = _articleRepository.ListAll(categoryId);
            return _mapper.Map<IList<ArticleInfo>>(articles);
        }

        public IList<ArticleInfo> FilterByRolesAndParent(IList<string>? roles, long? parentId)
        {
            var articles = _articleRepository.FilterByRolesAndParent(roles, parentId);
            return _mapper.Map<IList<ArticleInfo>>(articles);
        }

        public ArticleInfo GetById(int articleId)
        {
            var article = _articleRepository.GetById(articleId);
            return _mapper.Map<ArticleInfo>(article);
        }

        public ArticleInfo Insert(ArticleInfo article)
        {
            if (article == null)
                throw new ArgumentNullException(nameof(article));

            if (string.IsNullOrWhiteSpace(article.Title))
                throw new ArgumentException("Article title cannot be empty.", nameof(article.Title));

            if (string.IsNullOrWhiteSpace(article.Content))
                throw new ArgumentException("Article content cannot be empty.", nameof(article.Content));

            if (article.CategoryId <= 0)
                throw new ArgumentException("Article must have a valid category.", nameof(article.CategoryId));

            var articleModel = _mapper.Map<ArticleModel>(article);
            var insertedArticle = _articleRepository.Insert(articleModel);
            return _mapper.Map<ArticleInfo>(insertedArticle);
        }

        public ArticleInfo Update(ArticleInfo article)
        {
            if (article == null)
                throw new ArgumentNullException(nameof(article));

            if (string.IsNullOrWhiteSpace(article.Title))
                throw new ArgumentException("Article title cannot be empty.", nameof(article.Title));

            if (string.IsNullOrWhiteSpace(article.Content))
                throw new ArgumentException("Article content cannot be empty.", nameof(article.Content));

            if (article.CategoryId <= 0)
                throw new ArgumentException("Article must have a valid category.", nameof(article.CategoryId));

            var articleModel = _mapper.Map<ArticleModel>(article);
            var updatedArticle = _articleRepository.Update(articleModel);
            return _mapper.Map<ArticleInfo>(updatedArticle);
        }
    }
}
