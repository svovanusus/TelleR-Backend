using System;

namespace TelleR.Data.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Requested resource is not found.") { }
    }
}
