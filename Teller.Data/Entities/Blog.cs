using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TelleR.Data.Enums;

namespace TelleR.Data.Entities
{
    public class Blog
    {
        public Int64 Id { get; set; }

        [MaxLength(60)]
        [Required]
        public String Title { get; set; }

        [MaxLength(15000)]
        public String Description { get; set; }

        [DefaultValue(BlogType.Personal)]
        public BlogType Type { get; set; }

        [DefaultValue(false)]
        public Boolean IsPublic { get; set; }

        [Required]
        public virtual User Owner { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
    }
}
