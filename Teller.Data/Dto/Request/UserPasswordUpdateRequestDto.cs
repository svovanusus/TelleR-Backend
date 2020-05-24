using System;

namespace TelleR.Data.Dto.Request
{
    public class UserPasswordUpdateRequestDto
    {
        public String OldPassword { get; set; }
        public String NewPassword { get; set; }
        public String PasswordConfirm { get; set; }
    }
}
