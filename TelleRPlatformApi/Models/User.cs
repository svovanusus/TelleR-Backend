using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TelleRPlatformApi.Enums;

namespace TelleRPlatformApi.Models
{
    public class User
    {
        public Int64 Id { get; set; }

        [MaxLength(30)]
        [Required]
        public String FirstName { get; set; }

        [MaxLength(30)]
        [Required]
        public String SecondName { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(60)]
        [Required]
        public String Email { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(20)]
        [Required]
        public String Username { get; set; }

        [MaxLength(64)]
        [Required]
        public String Password { get; set; }

        [DefaultValue(false)]
        public Boolean IsActivate { get; set; }

        [DefaultValue(false)]
        public Boolean IsBlocked { get; set; }

        [DefaultValue(UserRole.User)]
        public UserRole Role { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime LastActive { get; set; }

    }
}
