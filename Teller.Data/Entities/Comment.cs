using System;
using System.ComponentModel.DataAnnotations;

namespace TelleR.Data.Entities
{
    public class Comment
    {
        public Int64 Id { get; set; }

        [MaxLength(1000)]
        [Required]
        public String Text { get; set; }

        [Required]
        public virtual User Author { get; set; }

        [Required]
        public virtual Post Post { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
    }
}
