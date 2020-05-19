using System;
using System.Collections.Generic;

namespace TelleR.Data.Dto.Response
{
    public class SignupResponseDto
    {
        public Boolean Status { get; set; }
        public IEnumerable<String> Messages { get; set; }
    }
}
