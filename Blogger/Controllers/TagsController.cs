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
    [Produces("application/json")]
    [Route("blog-api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private ITagServices _tagServices;

        public TagsController(ITagServices tagServices)
        {
            _tagServices = tagServices;
        }

        // GET: api/Tags
        [HttpGet("All")]
        public async Task<ActionResult> GetTags()
        {
            var tags = await _tagServices.GetAllTagsAsync().ConfigureAwait(false);
            if (tags is null)
                return NoContent();

            return Ok(tags);
        }


        // GET: api/Tags/5
        [HttpGet("Tags-{name}-{id}")]
        public async Task<ActionResult> GetTag(int id)
        {
            var Tag = await _tagServices.GetTagAsync(id).ConfigureAwait(false);// _context.Tags.FindAsync(id);

            if (Tag == null)
            {
                return NotFound();
            }

            return Ok(Tag);
        }

        // PUT: api/Tags/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("Tags-{id}")]
        public async Task<IActionResult> PutTag(int id, TagRequest Tag)
        {
            if (Tag == null || id != Tag.Id)
            {
                return BadRequest();
            }

            (int ResultAction, TagResponse UpdatedEntity) result = (-1, null);
            try
            {
                result = await _tagServices.UpdateTagAsync(id, Tag);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(id))
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

        // POST: api/Tags
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("Tags-add")]
        public async Task<ActionResult> PostTag(TagRequest Tag)
        {

            var addedTag = await _tagServices.AddTagAsync(Tag);
            //_context.Tags.Add(Tag);
            //await _context.SaveChangesAsync();

            return CreatedAtAction("Tags-{name}-{id}", new { name = addedTag.AddedEntity.Name, id = addedTag.AddedEntity.TagID }, addedTag.AddedEntity);
        }

        // DELETE: api/Tags/5
        [HttpDelete("Tags-{id}")]
        public async Task<ActionResult> DeleteTag(int id)
        {
            var Tag = await _tagServices.GetTagAsync(id);
            if (Tag == null)
            {
                return NotFound();
            }
            var result =await _tagServices.DeleteTagAsync(Tag);

            /*_context.Tags.Remove(Tag);
            await _context.SaveChangesAsync();*/

            return Ok(result.DeletedEntity);
        }

        private bool TagExists(int id)
        {
            return _tagServices.ISTagExist(id);
        }
    }
}
