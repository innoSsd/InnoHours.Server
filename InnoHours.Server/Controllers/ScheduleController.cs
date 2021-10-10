using System.Linq;
using System.Threading.Tasks;
using InnoHours.Server.Authentication;
using InnoHours.Server.Authentication.Annotations;
using Microsoft.AspNetCore.Mvc;
using InnoHours.Server.Database.Context;
using InnoHours.Server.Models.Schedule;

namespace InnoHours.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleDbContext _scheduleContext;

        public ScheduleController(ScheduleDbContext context)
        {
            _scheduleContext = context;
        }

        [HttpGet]
        [JwtAuthorized(UserRole = UserRole.Student)]
        public async Task<ActionResult<ScheduleForStudentResponse>> GetForStudent()
        {
            var group = HttpContext.User.Claims.First(claim => claim.Type == "Group").Value;
            var grade = HttpContext.User.Claims.First(claim => claim.Type == "Grade").Value;

            var scheduleResult = await _scheduleContext.GetScheduleForGroupAsync(grade, group);
            return new ScheduleForStudentResponse
            {
                Schedule = scheduleResult.Schedule,
                Grade = scheduleResult.Grade,
                Group = scheduleResult.Group
            };
        }

        [HttpGet]
        [JwtAuthorized(UserRole = UserRole.Student)]
        public async Task<ActionResult<ProfessorsForGradeResponse>> GetProfessors()
        {
            var group = HttpContext.User.Claims.First(claim => claim.Type == "Group").Value;
            var grade = HttpContext.User.Claims.First(claim => claim.Type == "Grade").Value;
            var professorsResult = await _scheduleContext.GetProfessorsForGradeAsync(grade, group);
            return new ProfessorsForGradeResponse
            {
                Professors = professorsResult
            };
        }

        [HttpGet]
        [JwtAuthorized(UserRole = UserRole.Professor)]
        public async Task<ActionResult<GroupsForProfessorResponse>> GetGroupsForProfessor()
        {
            var professorId = GetUserId();
            var groupResult = await _scheduleContext.GetAvailableGroupsForProfessorAsync(professorId);
            return new GroupsForProfessorResponse
            {
                Groups = groupResult.Groups
            };
        }

        private string GetUserId()
        {
            return HttpContext.User.Claims.First(claim => claim.Type == "Id").Value;
        }

    }
}
