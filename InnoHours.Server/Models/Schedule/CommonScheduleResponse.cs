using System.Collections.Generic;
using InnoHours.Server.Database.Data.Schedule.Common;

namespace InnoHours.Server.Models.Schedule
{
    public class CommonScheduleResponse
    {
        public IEnumerable<ScheduleDay> Schedule { get; set; }
    }
}