using System;
using TelleR.Data.Enums;

namespace TelleR.Logic.Services.Models
{
    public class AuthModel
    {
        public Int64 Id { get; set; }
        public String Username { get; set; }
        public UserRole Role { get; set; }
    }
}
