using Blogger.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogger.Context
{
    public interface IBloggerContext : IDbContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<Post> Posts { get; set; }
        DbSet<Blog> Blogs { get; set; }
    }
}
