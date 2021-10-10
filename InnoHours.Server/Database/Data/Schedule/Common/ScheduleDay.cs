using System.Collections.Generic;

namespace InnoHours.Server.Database.Data.Schedule.Common
{
    public class ScheduleDay
    {
        public string DayName { get; set; }

        public IEnumerable<ScheduleGrade> Grades { get; set; }
    }
}