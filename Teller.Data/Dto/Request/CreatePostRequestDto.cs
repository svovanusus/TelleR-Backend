using System;

namespace TelleR.Data.Dto.Request
{
    public class CreatePostRequestDto
    {
        public Int64? PostId { get; set; }

        public Int64 BlogId { get; set; }

        public String Title { get; set; }
        
        public String Content { get; set; }
        
        public Boolean IsPublished { get; set; }
    }
}
