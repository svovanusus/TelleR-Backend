using System;
using TelleR.Data.Enums;

namespace TelleR.Data.Dto.Request
{
    public class CreateBlogRequestDto
    {
        public Int64? BlogId { get; set; }
        public String Title { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public Boolean IsPublic { get; set; } = true;
        public BlogType Type { get; set; } = BlogType.Personal;
    }
}
