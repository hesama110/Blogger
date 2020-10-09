using Blogger.Model.Common;
using System;
using System.Collections.Generic;

namespace Blogger.Model
{
    public class Category:BaseEntity<int>
    {
        public Category()
        {
            Posts = new HashSet<Post>();

        }
        public string Name { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
