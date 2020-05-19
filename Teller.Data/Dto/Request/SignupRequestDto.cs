using System;

namespace TelleR.Data.Dto.Request
{
    public class SignupRequestDto
    {
        public String Username { get; set; }
        public String Password { get; set; }
        public String PasswordConfirm { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
    }
}
