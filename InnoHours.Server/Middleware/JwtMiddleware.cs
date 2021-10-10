using System.Linq;
using System.Threading.Tasks;
using InnoHours.Server.Authentication;
using Microsoft.AspNetCore.Http;

namespace InnoHours.Server.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserAuthenticator _authenticator;

        public JwtMiddleware(RequestDelegate next, IUserAuthenticator authenticator)
        {
            _next = next;
            _authenticator = authenticator;
        }

        public async Task InvokeAsync(HttpContext context, JwtHandler jwtHandler)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            var tokenValidation = jwtHandler.ValidateToken(token, out var userId);
            context.Items["token_validation"] = tokenValidation;
            context.Items["user_id"] = userId;
            await _next(context);

        }
    }
}