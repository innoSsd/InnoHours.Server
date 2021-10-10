using System.Collections.Generic;

namespace InnoHours.Server.Database.Data.Schedule
{
    public class AvailableGroupsForProfessor
    {
        public Dictionary<string, Dictionary<string, IEnumerable<string>>> Groups { get; set; }
    }
}