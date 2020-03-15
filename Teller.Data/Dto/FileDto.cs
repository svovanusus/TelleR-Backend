using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TelleR.Data.Dto
{
    public class FileDto
    {
        public String ContentType { get; set; }
        public String FileName { get; set; }
        public Stream ReadStream { get; set; }
    }
}
