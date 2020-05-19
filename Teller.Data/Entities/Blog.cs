using System;
using System.Collections.Generic;
using TelleR.Data.Enums;

namespace TelleR.Data.Entities
{
    public class Blog
    {
        public Int64 Id { get; set; }

        public String Name { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        public BlogType Type { get; set; }

        public Boolean IsPublic { get; set; }

        public virtual User Owner { get; set; }

        public virtual IEnumerable<Post> Posts { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
