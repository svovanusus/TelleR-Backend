using System;

namespace TelleR.Data.Exceptions
{
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException() : base("User douesn't have permissions to do this action or get requested resource.") { }
    }
}
