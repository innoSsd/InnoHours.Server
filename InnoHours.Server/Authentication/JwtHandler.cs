using System;
using System.Security.Cryptography;
using System.Text;
using InnoHours.Server.Authentication.Jwt;
using InnoHours.Server.DataBase.Models;
using InnoHours.Server.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace InnoHours.Server.Authentication
{
    public class JwtHandler
    {
        private readonly AppSettings _settings;

        public JwtHandler(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        public string CreateToken(Student student, string platform = "web")
        {
            var header = new JwtHeader
            {
                Algorithm = _settings.DefaultHashAlgorithm,
                Host = _settings.Host,
                Platform = platform
            };
            var payload = new JwtPayload
            {
                CreatedAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddYears(100),
                UserId = student.Id
            };
            return CreateToken(header, payload);
        }

        public string CreateToken(Professor professor, string platform = "web")
        {
            var head = new JwtHeader
            {
                Algorithm = _settings.DefaultHashAlgorithm,
                Host = _settings.Host,
                Platform = platform
            };
            var payload = new JwtPayload
            {
                CreatedAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddYears(100),
                UserId = professor.Id
            };
            return CreateToken(head, payload);
        }

        public TokenValidation ValidateToken(string token, out string userId)
        {
            userId = null;
            var tokenParts = token?.Split('.') ?? Array.Empty<string>();
            if (tokenParts.Length != 3) return TokenValidation.Invalid;

            //validating token
            var sign = SignToken(tokenParts[0], tokenParts[1]);
            if (tokenParts[2] != sign) return TokenValidation.Invalid;

            var headerJson = Encoding.UTF8.GetString(Convert.FromBase64String(tokenParts[0]));
            var payloadJson = Encoding.UTF8.GetString(Convert.FromBase64String(tokenParts[1]));

            try
            {
                var header = JsonConvert.DeserializeObject<JwtHeader>(headerJson);
                var payload = JsonConvert.DeserializeObject<JwtPayload>(payloadJson);
                if (header == null || payload == null) return TokenValidation.Invalid;

                //check for expiration
                if (payload.ExpiredAt < DateTime.Now) return TokenValidation.Expired;
                userId = payload.UserId;
                return TokenValidation.Valid;
            }
            catch (Exception)
            {
                return TokenValidation.Invalid;
            }
        }

        private string CreateToken(JwtHeader header, JwtPayload payload)
        {
            if (header.Algorithm != "SHA256") throw new Exception($"Uknown algorithm {header.Algorithm}");
            
            var headerJson = JsonConvert.SerializeObject(header);
            var payloadJson = JsonConvert.SerializeObject(payload);
            var headerBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(headerJson));
            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));
            var token = $"{headerBase64}.{payloadBase64}.{SignToken(headerBase64, payloadBase64)}";
            return token;
        }

        private string SignToken(string header, string payload)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_settings.JwtSecret);
            using var cipher = new HMACSHA256(keyBytes);
            var token = $"{header}.{payload}";
            var hashedToken = cipher.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hashedToken);
        }

        public enum TokenValidation
        {
            Valid, Invalid, Expired
        }
    }
}