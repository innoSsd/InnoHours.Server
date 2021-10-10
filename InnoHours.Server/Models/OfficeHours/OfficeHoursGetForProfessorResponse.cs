using System.Collections.Generic;
using InnoHours.Server.Database.Data.OfficeHours;

namespace InnoHours.Server.Models.OfficeHours
{
    public class OfficeHoursGetForProfessorResponse
    {
        public IEnumerable<OfficeHoursForProfessor> OfficeHours { get; set; }
    }
}