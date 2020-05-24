using System;

namespace TelleR.Data.Dto.Response
{
    public class BlogResponseDto
    {
        public Int64 Id { get; set; }
        public String Title { get; set; }
        public UserResponseDto Author { get; set; }
        public Int64 PostsCount { get; set; }
    }
}
