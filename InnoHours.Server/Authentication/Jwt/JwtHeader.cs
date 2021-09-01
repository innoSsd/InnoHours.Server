using System;

namespace InnoHours.Server.Authentication.Jwt
{
    public class JwtHeader
    {
        public string Platform { get; set; }

        public string Host { get; set; }

        public string Algorithm { get; set; }

    }
}