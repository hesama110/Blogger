using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blogger.Context;
using Blogger.Model;
using Blogger.Services;
using Blogger.MessagesModel;
using System.Text.RegularExpressions;

namespace Blogger.Controllers
{
    /// <summary>
    /// All Apis relate to category 
    /// </summary>
    [Produces("application/json")]
    [Route("blog-api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryServices _categoryServices;

        public CategoriesController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        // GET: api/Categories
        /// <summary>
        /// Get All visible categories
        /// 
        /// </summary>
        /// <returns>List of categories</returns>
        [HttpGet("All")]
        public async Task<ActionResult> GetCategories()
        {
            var categories = await _categoryServices.GetAllCategoryAsync().ConfigureAwait(false);
            if (categories is null)
                return NoContent();

            return Ok(categories);
        }


        // GET: api/Categories/5
        /// <summary>
        /// Get detail of a category
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <returns>Category detail</returns>
        [HttpGet("Category-{name}-{id}")]
        public async Task<ActionResult> GetCategory(int id)
        {
            var category = await _categoryServices.GetCategoryAsync(id).ConfigureAwait(false);// _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/Categories/5
        /// <summary>
        /// Update a category
        /// </summary>
        /// <param name="id">ID category need to be update</param>
        /// <param name="category">category object</param>
        /// <returns>Updated category object</returns>
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("Category-{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryRequest category)
        {
            if (category == null || id != category.Id)
            {
                return BadRequest();
            }

            (int ResultAction, CategoryResponse UpdatedEntity) result = (-1, null);
            try
            {
                result = await _categoryServices.UpdateCategoryAsync(id, category);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(result.UpdatedEntity);
            //return NoContent();
        }

        // POST: api/Categories
        /// <summary>
        /// Add new category
        /// </summary>
        /// <param name="category">Detail new category object</param>
        /// <returns>Added category</returns>
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("Category-add")]
        public async Task<ActionResult> PostCategory(CategoryRequest category)
        {

            var addedCategory = await _categoryServices.AddCategoryAsync(category);
           
            return CreatedAtAction("Category-{name}-{id}", new { name = addedCategory.AddedEntity.Name, id = addedCategory.AddedEntity.CatID }, addedCategory.AddedEntity);
        }

        // DELETE: api/Categories/5
        /// <summary>
        /// delete a category
        /// </summary>
        /// <param name="id">Id Category need to delete</param>
        /// <returns></returns>
        [HttpDelete("Category-{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var category = await _categoryServices.GetCategoryAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var result =await _categoryServices.DeleteCategoryAsync(category);

            /*_context.Categories.Remove(category);
            await _context.SaveChangesAsync();*/

            return Ok(result.DeletedEntity);
        }

        private bool CategoryExists(int id)
        {
            return _categoryServices.ISCategoryExist(id);
        }
    }
}
