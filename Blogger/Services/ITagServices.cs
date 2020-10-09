using Blogger.MessagesModel;
using Blogger.Model;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Services
{
    public interface ITagServices
    {
        Task<IEnumerable<TagResponse>> GetAllTagsAsync();
        bool ISTagExist(int id);
        Task<TagResponse> GetTagAsync(int id);
        Task<(int ResultAction, TagResponse UpdatedEntity)> UpdateTagAsync(int id, TagRequest categoryRequest);
        Task<(int ResultAction, TagResponse AddedEntity)> AddTagAsync(TagRequest category);
        Task<(int ResultAction, TagResponse DeletedEntity)> DeleteTagAsync(TagResponse category);
    }
}
