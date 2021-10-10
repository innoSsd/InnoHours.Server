using System.Collections.Generic;
using InnoHours.Server.Database.Data.Schedule;

namespace InnoHours.Server.Models.Schedule
{
    public class GroupsForProfessorResponse
    {
        public Dictionary<string, Dictionary<string, IEnumerable<string>>> Groups { get; set; }
    }
}