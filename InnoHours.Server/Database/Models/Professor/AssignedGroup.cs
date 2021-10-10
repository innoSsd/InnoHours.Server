using MongoDB.Bson.Serialization.Attributes;

namespace InnoHours.Server.Database.Models.Professor
{
    public class AssignedGroup
    {
        [BsonElement("course_name")]
        public string CourseName { get; set; }

        [BsonElement("professor_type")]
        public string ProfessorType { get; set; }

        [BsonElement("grade")]
        public string Grade { get; set; }

        [BsonElement("group")]
        public string Group {  get; set; }
    }
}