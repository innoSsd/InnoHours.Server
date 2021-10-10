using System.Collections.Generic;
using InnoHours.Server.Database.Data.Schedule;

namespace InnoHours.Server.Models.Schedule
{
    public class ProfessorsForGradeResponse
    {
        public IEnumerable<ProfessorForGrade> Professors { get; set; }
    }
}