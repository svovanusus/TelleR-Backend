using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TelleRPlatformApi.Models
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
