using System;

namespace InnoHours.Server.Database.Data.OfficeHours
{
    public class OfficeHoursForStudent
    {
        public string Id { get; set; }

        public string Grade { get; set; }

        public string Group { get; set; }

        public string CourseName { get; set; }

        public int StudentsLimit { get; set; }

        public int AppliedStudentsCount { get; set; }

        public string ProfessorName { get; set; }

        public string ProfessorId { get; set; }

        public string Title { get; set; }

        public string Description {  get; set; }

        public DateTime Date { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Location { get; set; }

        public bool IsApplied { get; set; }

    }
}