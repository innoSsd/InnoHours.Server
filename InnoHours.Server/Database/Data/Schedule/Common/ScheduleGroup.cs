using System.Collections.Generic;

namespace InnoHours.Server.Database.Data.Schedule.Common
{
    public class ScheduleGroup
    {
        public string GroupName { get; set; }

        public IEnumerable<ScheduleLesson> Lessons { get; set; }
    }
}