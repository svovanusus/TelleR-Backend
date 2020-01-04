using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace TelleRPlatformApi
{
    public static class AuthConfig
    {
        public const String ISSUER = "TelleR-SERVER";

        public const String AUDIENCE = "TelleR-CLIENT";

        private const String KEY = "Super Secret String";

        public const UInt64 LIFETIME = 2592000;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
