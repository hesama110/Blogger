using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blogger.Context;
using Blogger.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blogger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BloggerController : Controller
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly BloggerContext _context;

        public BloggerController(BloggerContext context,ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _context = context;
            var test=_context.Categories.ToList();
        }
        
        [HttpGet]
        public IEnumerable<Category> GetCategories()
        {
            return null;
        }

    }
}
