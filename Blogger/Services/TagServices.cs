using Blogger.Context;
using Blogger.MessagesModel;
using Blogger.Model;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Blogger.Services
{
    public class TagServices : ITagServices
    {
        private IBaseServices<Tag> _tagBaseServices;

        public TagServices(IBaseServices<Tag> tagBaseServices)
        {
            _tagBaseServices = tagBaseServices;
        }


        public async Task<IEnumerable<TagResponse>> GetAllTagsAsync()
        {
            List<TagResponse> categoriesResponse = new List<TagResponse>();
            var categories = await _tagBaseServices.GetListAsync(x => x.Visible).ConfigureAwait(false);
            if (categories is null)
                return null;
            foreach (var cat in categories)
            {
                categoriesResponse.Add(new TagResponse { TagID = cat.Id, Name = cat.Name });
            }
            return categoriesResponse;
        }

        public bool ISTagExist(int id)
        {
            return _tagBaseServices.Query(null, null).Any(x => x.Id == id);
        }

        public async Task<TagResponse> GetTagAsync(int id)
        {
            var tag = await _tagBaseServices.GetFirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            if (tag is null)
                return null;
            return new TagResponse { TagID = tag.Id, Name = tag.Name };
        }

        public async Task<(int ResultAction, TagResponse UpdatedEntity)> UpdateTagAsync(int id, TagRequest tagRequest)
        {
            if (tagRequest == null)
                return (-1, null);
            var updateTag = new Tag() { Id = tagRequest.Id, Name = tagRequest.Name, Visible = tagRequest.Visible, EditedAt = DateTime.Now };

            var result = await _tagBaseServices.UpdateAsync(updateTag, x => x.Name, x => x.Visible, x => x.EditedAt).ConfigureAwait(false);

            return (result.ResultAction, new TagResponse { TagID = result.UpdatedEntity.Id, Name = result.UpdatedEntity.Name });
        }

        public async Task<(int ResultAction, TagResponse AddedEntity)> AddTagAsync(TagRequest tag)
        {
            if (tag == null)
                return (-1, null);
            var addTag = new Tag() { Name = tag.Name, Visible = tag.Visible };

            var result = await _tagBaseServices.InsertAsync(addTag).ConfigureAwait(false);
            return (result.ResultAction, new TagResponse { TagID = result.AddedEntity.Id, Name = result.AddedEntity.Name });

        }

        public async Task<(int ResultAction, TagResponse DeletedEntity)> DeleteTagAsync(TagResponse tag)
        {
            if (tag == null)
                return (-1,null);
            var result = await _tagBaseServices.DeleteAsync(tag.TagID).ConfigureAwait(false);
            return (result.ResultAction, new TagResponse { TagID = result.DeletedEntity.Id, Name = result.DeletedEntity.Name });
        }
    }
}
