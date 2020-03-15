using System;

namespace TelleR.Data.Dto.Response
{
    public class PostResponseDto
    {
        public Int64 Id { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        public String Content { get; set; }

        public UserResponseDto Author { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
