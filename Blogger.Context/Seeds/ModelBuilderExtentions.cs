using Blogger.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogger.Context.Seeds
{
    public static class ModelBuilderExtentions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "C#", Visible = true },
                new Category { Id = 2, Name = "SQL", Visible = true },
                new Category { Id = 3, Name = "Entity Framework", Visible = true },
                new Category { Id = 4, Name = "MVC", Visible = true },
                new Category { Id = 5, Name = "SignalR", Visible = true },
                new Category { Id = 6, Name = "HideMe", Visible = false }

                );

            modelBuilder.Entity<Tag>().HasData(
                new Tag { Id = 1, Name = "MVC", Visible = true },
                new Tag { Id = 2, Name = "C#", Visible = true },
                new Tag { Id = 3, Name = "EF", Visible = true },
                new Tag { Id = 4, Name = "CSharp", Visible = true },
                new Tag { Id = 5, Name = "SQL", Visible = true },
                new Tag { Id = 6, Name = "SQL Managements", Visible = true },
                new Tag { Id = 7, Name = "Signalr", Visible = true },
                new Tag { Id = 8, Name = "HideMe", Visible = false }

                );
        }
    }
}
