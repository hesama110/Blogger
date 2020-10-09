using Attributes;
using Blogger.Model.Common;
using Blogger.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blogger.Model
{
   public class Post:BaseEntity<int>
    {

        public int? BlogParentID { get; set; }
        [Required]
        public string Title { get; set; }
        
        [Required]
        [DataType(DataType.Html)]
        public string Content { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Tag> Tags { get; set; }

        public BlogStatus Status { get; set; }
        [DefaultDateTimeValue("Now")] 
        public DateTime PublishFrom { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }

    }
}
