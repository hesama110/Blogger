using Blogger.MessagesModel;
using Blogger.Model;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Services
{
    public interface ICategoryServices
    {
        Task<IEnumerable<CategoryResponse>> GetAllCategoryAsync();
        bool ISCategoryExist(int id);
        Task<CategoryResponse> GetCategoryAsync(int id);
        Task<(int ResultAction, CategoryResponse UpdatedEntity)> UpdateCategoryAsync(int id, CategoryRequest categoryRequest);
        Task<(int ResultAction, CategoryResponse AddedEntity)> AddCategoryAsync(CategoryRequest category);
        Task<(int ResultAction, CategoryResponse DeletedEntity)> DeleteCategoryAsync(CategoryResponse category);
    }
}
