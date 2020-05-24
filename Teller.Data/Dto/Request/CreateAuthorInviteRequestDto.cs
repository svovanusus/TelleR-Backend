using System;

namespace TelleR.Data.Dto.Request
{
    public class CreateAuthorInviteRequestDto
    {
        public Int64 BlogId { get; set; }
        public String Email { get; set; }
    }
}
