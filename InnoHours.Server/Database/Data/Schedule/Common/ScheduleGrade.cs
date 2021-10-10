using System.Collections.Generic;

namespace InnoHours.Server.Database.Data.Schedule.Common
{
    public class ScheduleGrade
    {
        public string GradeName { get; set; }

        public IEnumerable<ScheduleGroup> Groups { get; set; }
    }
}