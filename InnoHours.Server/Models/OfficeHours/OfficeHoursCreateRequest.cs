using System;

namespace InnoHours.Server.Models.OfficeHours
{
    public class OfficeHoursCreateRequest
    {
        public string CourseName { get; set; }

        public string Title { get; set; }

        public string Description {  get; set; }

        public DateTime Date { get; set; }

        public string StartTime { get; set; }

        public string EndTime {  get; set; }

        public string Grade { get; set; }

        public string Group { get; set; }

        public string RepeatEvery { get; set; }

        public int StudentsLimit { get; set; }

        public string Location { get; set; }
    }
}