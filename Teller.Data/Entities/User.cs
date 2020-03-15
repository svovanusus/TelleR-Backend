using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TelleR.Data.Enums;

namespace TelleR.Data.Entities
{
    public class User
    {
        public Int64 Id { get; set; }

        [MaxLength(30)]
        [Required]
        public String FirstName { get; set; }

        [MaxLength(30)]
        [Required]
        public String LastName { get; set; }

        [MaxLength(60)]
        [Required]
        public String Email { get; set; }

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
