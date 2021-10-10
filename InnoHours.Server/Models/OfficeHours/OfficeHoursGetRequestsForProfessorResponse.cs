using System.Collections.Generic;
using InnoHours.Server.Database.Data.OfficeHours;

namespace InnoHours.Server.Models.OfficeHours
{
    public class OfficeHoursGetRequestsForProfessorResponse
    {
        public IEnumerable<OfficeHoursProfessorRequest> Requests { get; set; }
    }
}