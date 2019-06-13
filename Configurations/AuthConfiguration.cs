using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AllNewsServer.Configurations
{
    public class AuthConfiguration
    {
        public const string TOKEN_ISSUER = "MyAuthServer";

        public const string TOKEN_AUDIENCE = "https://my.allnews.app/";

        const string TOKEN_KEY = "mysupersecret_secretkey!123";

        public const int TOKEN_LIFETIME = 60; // in minutes

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(TOKEN_KEY));
        }

        public const int ACCESS_CODE_LIFETIME = 2; // in minutes

        public const int ACCESS_CODE_RENEWAL = 1; // in minutes
    }
}
