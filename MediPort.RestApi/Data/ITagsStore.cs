using MediPortApi.HttpProcessing;
using MediPortApi.TagProcessing;

namespace MediPort.RestApi.Data
{
    public interface ITagsStore
    {
        Task<IEnumerable<SimplifiedTag>> GetAllTags();
        Task<SimplifiedTag> GetTag(int id);
        Task<SimplifiedTag> CreateTag(Tag tag);
        Task UpdateTag(int id, Tag tag);
        Task DeleteTag(int id);
    }
}
