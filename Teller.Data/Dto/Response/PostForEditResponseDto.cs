using System;

namespace TelleR.Data.Dto.Response
{
    public class PostForEditResponseDto
    {
        public Int64 PostId { get; set; }
        public String Title { get; set; }
        public String PostContent { get; set; }
        public Boolean IsPublished { get; set; }
        public DateTime? PublishedDate { get; set; }
    }
}
