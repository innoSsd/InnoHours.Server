using System;

namespace InnoHours.Server.Database.Data.OfficeHours
{
    public class OfficeHoursCreating
    {
        public string Title { get; set; }

        public string Description {  get; set; }

        public string Location { get; set; }

        public DateTime Date { get; set; }

        public string TimeStart { get; set; }

        public string TimeEnd {  get; set; }

        public string RepeatEvery { get; set; }

        public string Grade { get; set; }

        public string Group { get; set; }

        public int StudentsLimit { get; set; }

        public string CourseName { get; set; }


    }
}