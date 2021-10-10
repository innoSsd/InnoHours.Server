using MongoDB.Bson.Serialization.Attributes;

namespace InnoHours.Server.Database.Models.Schedule
{
    public class ScheduleCourse
    {
        [BsonElement("course_name")]
        public string CourseName { get; set; }

        [BsonElement("lesson_type")]
        public string LessonType { get; set; }
    }
}