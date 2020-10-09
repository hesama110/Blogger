using Blogger.Model.Common;
using Blogger.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blogger.Model
{
   public class Blog: BaseEntity<int>
    {
        [Required]
        public string Name { get; set; }
        public string Author { get; set; }
        public ICollection<Post> Posts { get; set; }

    }
}
