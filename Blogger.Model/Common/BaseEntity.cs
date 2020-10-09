using Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blogger.Model.Common
{
    public abstract class BaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
        public bool Visible { get; set; }

        [DefaultDateTimeValue("Now")] 
        public DateTime InsertedAt { get; set; } = default(DateTime);

        [DefaultDateTimeValue("Now")] 
        public DateTime EditedAt { get; set; } = default(DateTime);
    }
}
