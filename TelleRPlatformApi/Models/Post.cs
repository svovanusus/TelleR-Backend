using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TelleRPlatformApi.Models
{
    public class Post
    {
        public Int64 Id { get; set; }

        [MaxLength(60)]
        [Required]
        public String Title { get; set; }

        [MaxLength(1000)]
        public String Description { get; set; }

        [Required]
        public String PostContent { get; set; }

        [DefaultValue(false)]
        public Boolean IsPublished { get; set; }

        public DateTime PublishDate { get; set; }

        public DateTime UpdateDate { get; set; }

        [Required]
        public virtual User Author { get; set; }
    }
}
