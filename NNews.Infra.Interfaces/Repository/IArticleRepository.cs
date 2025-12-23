namespace NNews.Infra.Interfaces.Repository
{
    public interface IArticleRepository<TModel>
    {
        IEnumerable<TModel> ListAll(long? categoryId);
        IEnumerable<TModel> FilterByRolesAndParent(IList<string>? roles, long? parentId);
        TModel GetById(int id);
        int CountByCategoryId(int categoryId);
        TModel Insert(TModel category);
        TModel Update(TModel category);
        void Delete(int id);
    }
}
