using System;

namespace TelleR.Data.Dto.Response
{
    public class DictionaryItemDto<T>
    {
        public String label { get; set; }
        public T value { get; set; }
    }
}
