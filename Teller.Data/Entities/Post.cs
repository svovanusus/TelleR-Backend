using System;
using System.Collections.Generic;

namespace TelleR.Data.Entities
{
    public class Post
    {
        public Int64 Id { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        public String PostContent { get; set; }

        public Boolean IsPublished { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime PublishDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public virtual Blog Blog { get; set; }

        public virtual User Author { get; set; }

        public virtual IEnumerable<Comment> Comments { get; set; }
    }
}
