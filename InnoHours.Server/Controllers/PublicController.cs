using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using InnoHours.Server.Database.Context;
using InnoHours.Server.Models.Public;
using InnoHours.Server.Models.Schedule;

namespace InnoHours.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PublicController : ControllerBase
    {
        private readonly ScheduleDbContext _scheduleContext;

        public PublicController(ScheduleDbContext context)
        {
            _scheduleContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<AvailableGroupsResponse>> AvailableGroups()
        {
            var groups = await _scheduleContext.GetAvailableGroups();
            return new AvailableGroupsResponse
            {
                Groups = groups.Groups
            };
        }

        [HttpGet]
        public async Task<ActionResult<CommonScheduleResponse>> GetSchedule()
        {
            var commonScheduleResult = await _scheduleContext.GetCommonScheduleAsync();
            return new CommonScheduleResponse
            {
                Schedule = commonScheduleResult
            };
        }
    }
}
