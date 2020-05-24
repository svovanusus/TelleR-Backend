using System;

namespace TelleR.Data.Entities
{
    public class BlogAuthor
    {
        public Int64 BlogId { get; set; }
        public virtual Blog Blog { get; set; }

        public Int64 AuthorId { get; set; }
        public virtual User Author { get; set; }
    }
}
