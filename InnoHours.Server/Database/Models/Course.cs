using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InnoHours.Server.DataBase.Models
{
    public class Course
    {
        [BsonId] [BsonElement("course_id")] [BsonRepresentation(BsonType.ObjectId)]
        public string CourseId { get; set; }

        [BsonElement("course_name")]
        public string CourseName {  get; set; }

    }
}