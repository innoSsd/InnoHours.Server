using System.Collections.Generic;
using InnoHours.Server.Database.Data.OfficeHours;

namespace InnoHours.Server.Models.OfficeHours
{
    public class OfficeHoursGetRequestsForStudentResponse
    {
        public IEnumerable<OfficeHoursStudentRequest> Requests { get; set; }
    }
}