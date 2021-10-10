using System.Collections.Generic;

namespace InnoHours.Server.Database.Data.Schedule
{
    public class AvailableGroups
    {
        public Dictionary<string, IEnumerable<string>> Groups { get; set; }
    }
}