using Blogger.Context.Seeds;
using Blogger.Model;
using Blogger.Model.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Blogger.Context
{
    /*public class LogDbContext : DbContext, ILogDbContext
    {
        public AppDbContext(DbContextOptions<LogDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
    }*/
    public class BloggerContext : DbContext, IBloggerContext
    {
        public BloggerContext(DbContextOptions<BloggerContext> options) : base(options) { }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Email>().Property(p => p.ID).HasDefaultValueSql("newid()");
            modelBuilder.Entity<Post>().Property(x => x.InsertedAt).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Post>().Property(x => x.EditedAt).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Post>().Property(x => x.PublishFrom).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Post>().HasMany(x => x.Tags);
            modelBuilder.Entity<Post>().HasMany(x => x.Categories);
            modelBuilder.Entity<Category>().HasMany(x => x.Posts);
            modelBuilder.Entity<Category>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Tag>().HasMany(x => x.Posts);
            modelBuilder.Entity<Tag>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Blog>().Property(x => x.InsertedAt).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Blog>().Property(x => x.EditedAt).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Tag>().Property(x => x.InsertedAt).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Tag>().Property(x => x.EditedAt).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Category>().Property(x => x.InsertedAt).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Category>().Property(x => x.EditedAt).HasDefaultValueSql("getdate()");

            /*.HasMany(x => x.Tags)
            .many(x => x.Posts);*/
            //modelBuilder.Entity<Post>();
            /*     .HasMany(x => x.Categories)
                 .WithOne(x => x.Post);*/
            //  modelBuilder.Entity<Blog>();
            modelBuilder.Seed();
        }

       
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BloggerContext>
    {
        public BloggerContext CreateDbContext(string[] args)
        {
            var test = Directory.GetParent(AppContext.BaseDirectory).FullName;
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())

                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../Blogger/appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<BloggerContext>();
            var connectionString = configuration.GetConnectionString("BloggerContextConnection");
            builder.UseSqlServer(connectionString);

            return new BloggerContext(builder.Options);
        }
    }
}
