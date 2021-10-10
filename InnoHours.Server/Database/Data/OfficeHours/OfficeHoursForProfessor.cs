using System;
using System.Collections.Generic;
using InnoHours.Server.Database.Models.Professor;

namespace InnoHours.Server.Database.Data.OfficeHours
{
    public class OfficeHoursForProfessor
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description {  get; set; }

        public string Grade { get; set; }

        public string Group { get; set; }

        public DateTime Date { get; set; }

        public string StartTime { get; set; }

        public string EndTime {  get; set; }

        public string Location { get; set; }

        public string CourseName { get; set; }

        public int StudentsLimit { get; set; }

        public IEnumerable<AppliedStudent> AppliedStudents { get; set; }

        public string RepeatEvery { get; set; }
    }
}