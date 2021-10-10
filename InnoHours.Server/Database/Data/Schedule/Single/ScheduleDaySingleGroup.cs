using System.Collections.Generic;
using InnoHours.Server.Database.Data.Schedule.Common;

namespace InnoHours.Server.Database.Data.Schedule.Single
{
    public class ScheduleDaySingleGroup
    {
        public string DayName { get; set; }

        public IEnumerable<ScheduleLesson> Lessons {  get; set; }
    }
}