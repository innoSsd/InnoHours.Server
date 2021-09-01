using System;

namespace InnoHours.Server.Authentication.Jwt
{
    public class JwtPayload
    {
        public DateTime CreatedAt { get; set; }

        public DateTime ExpiredAt { get; set; }

        public string UserId { get; set; }
    }
}