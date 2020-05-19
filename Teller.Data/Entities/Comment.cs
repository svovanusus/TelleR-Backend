using System;

namespace TelleR.Data.Entities
{
    public class Comment
    {
        public Int64 Id { get; set; }

        public String Text { get; set; }

        public virtual User Author { get; set; }

        public virtual Post Post { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
