using System.Threading.Tasks;
using InnoHours.Server.Authentication;
using InnoHours.Server.Models.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace InnoHours.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtHandler _jwtHandler;
        private readonly IUserAuthenticator _authenticator;

        public AuthController(JwtHandler handler, IUserAuthenticator authenticator)
        {
            _jwtHandler = handler;
            _authenticator = authenticator;
        }

        [HttpPost]
        public async Task<ActionResult> CreateSession([FromBody] AuthenticationRequest authData)
        {
            var student = await _authenticator.AuthenticateStudentAsync(authData.Email, authData.PasswordHash);
            if (student != null)
            {
                var response = new AuthenticationResponse
                {
                    UserFullName = student.FullName,
                    UserId = student.Id,
                    UserType = "student",
                    Token = _jwtHandler.CreateToken(student, authData.Platform)
                };
                return Ok(response);
            }

            var professor = await _authenticator.AuthenticateProfessorAsync(authData.Email, authData.PasswordHash);
            if (professor != null)
            {
                var response = new AuthenticationResponse
                {
                    UserFullName = professor.FullName,
                    UserId = professor.Id,
                    UserType = "professor",
                    Token = _jwtHandler.CreateToken(professor, authData.Platform)
                };
                return Ok(response);
            }

            return Unauthorized();
        }
    }
}