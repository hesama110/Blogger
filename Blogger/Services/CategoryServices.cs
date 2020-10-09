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
    public class CategoryServices : ICategoryServices
    {
        private IBaseServices<Category> _categoryBaseServices;

        public CategoryServices(IBaseServices<Category> categoryBaseServices)
        {
            _categoryBaseServices = categoryBaseServices;
        }


        public async Task<IEnumerable<CategoryResponse>> GetAllCategoryAsync()
        {
            List<CategoryResponse> categoriesResponse = new List<CategoryResponse>();
            var categories = await _categoryBaseServices.GetListAsync(x => x.Visible).ConfigureAwait(false);
            if (categories is null)
                return null;
            foreach (var cat in categories)
            {
                categoriesResponse.Add(new CategoryResponse { CatID = cat.Id, Name = cat.Name });
            }
            return categoriesResponse;
        }

        public bool ISCategoryExist(int id)
        {
            return _categoryBaseServices.Query(null, null).Any(x => x.Id == id);
        }

        public async Task<CategoryResponse> GetCategoryAsync(int id)
        {
            var category = await _categoryBaseServices.GetFirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            if (category is null)
                return null;
            return new CategoryResponse { CatID = category.Id, Name = category.Name };
        }

        public async Task<(int ResultAction, CategoryResponse UpdatedEntity)> UpdateCategoryAsync(int id, CategoryRequest categoryRequest)
        {
            if (categoryRequest == null)
                return (-1, null);
            var updateCategory = new Category() { Id = categoryRequest.Id, Name = categoryRequest.Name, Visible = categoryRequest.Visible, EditedAt = DateTime.Now };

            var result = await _categoryBaseServices.UpdateAsync(updateCategory, x => x.Name, x => x.Visible, x => x.EditedAt).ConfigureAwait(false);

            return (result.ResultAction, new CategoryResponse { CatID = result.UpdatedEntity.Id, Name = result.UpdatedEntity.Name });
        }

        public async Task<(int ResultAction, CategoryResponse AddedEntity)> AddCategoryAsync(CategoryRequest category)
        {
            if (category == null)
                return (-1, null);
            var addCategory = new Category() { Name = category.Name, Visible = category.Visible };

            var result = await _categoryBaseServices.InsertAsync(addCategory).ConfigureAwait(false);
            return (result.ResultAction, new CategoryResponse { CatID = result.AddedEntity.Id, Name = result.AddedEntity.Name });

        }

        public async Task<(int ResultAction, CategoryResponse DeletedEntity)> DeleteCategoryAsync(CategoryResponse category)
        {
            if (category == null)
                return (-1,null);
            var result = await _categoryBaseServices.DeleteAsync(category.CatID).ConfigureAwait(false);
            return (result.ResultAction, new CategoryResponse { CatID = result.DeletedEntity.Id, Name = result.DeletedEntity.Name });
        }
    }
}
