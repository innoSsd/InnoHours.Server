using System.Collections.Generic;
using InnoHours.Server.Database.Data.OfficeHours;

namespace InnoHours.Server.Models.OfficeHours
{
    public class OfficeHoursGetForStudentResponse
    {
        public IEnumerable<OfficeHoursForStudent> OfficeHours { get; set; }
    }
}