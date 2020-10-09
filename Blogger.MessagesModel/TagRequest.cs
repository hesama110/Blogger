using System;
using System.Collections.Generic;
using System.Text;

namespace Blogger.MessagesModel
{
   public class TagRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Visible { get; set; }
    }
}
