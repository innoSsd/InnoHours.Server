using System.Collections.Generic;
using InnoHours.Server.Database.Data.Schedule.Single;

namespace InnoHours.Server.Models.Schedule
{
    public class ScheduleForStudentResponse
    {
        public IEnumerable<ScheduleDaySingleGroup> Schedule { get; set; }

        public string Grade { get; set; }

        public string Group { get; set; }
    }
}