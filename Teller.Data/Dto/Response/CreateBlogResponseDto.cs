using System;
using System.Collections.Generic;

namespace TelleR.Data.Dto.Response
{
    public class CreateBlogResponseDto
    {
        public Boolean IsError { get; set; }
        public IEnumerable<String> Messages { get; set; }
    }
}
