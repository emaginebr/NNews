using NNews.Dtos;

namespace NNews.Domain.Services.Interfaces
{
    public interface ITagService
    {
        IList<TagInfo> ListAll();
        TagInfo GetById(int tagId);
        Task<TagInfo> InsertAsync(TagInfo tag);
        Task<TagInfo> UpdateAsync(TagInfo tag);
        void Delete(int tagId);
        void MergeTags(long sourceTagId, long targetTagId);
    }
}
