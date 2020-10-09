using Blogger.Model.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogger.Model
{
    public class Tag:BaseEntity<int>
    {
        public Tag()
        {
            Posts = new HashSet<Post>();

        }
        public string Name { get; set; }
        public ICollection<Post> Posts { get; set; }

    }
}
