using System.Collections.Generic;

namespace InnoHours.Server.Database.Data.Schedule.Single
{
    public class ScheduleSingleGroup
    {
        public string Grade { get; set; }

        public string Group { get; set; }

        public IEnumerable<ScheduleDaySingleGroup> Schedule { get; set; }
    }
}