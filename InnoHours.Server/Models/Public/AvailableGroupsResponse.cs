using System.Collections.Generic;

namespace InnoHours.Server.Models.Public
{
    public class AvailableGroupsResponse
    {
        public Dictionary<string, IEnumerable<string>> Groups { get; set; }
    }
}