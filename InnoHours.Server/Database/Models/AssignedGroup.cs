using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InnoHours.Server.DataBase.Models
{
    public class AssignedGroup
    {
        [BsonId] [BsonElement("id")] [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("group_type_id")] [BsonRepresentation(BsonType.ObjectId)]
        public string GroupTypeId { get; set; }

        [BsonElement("professor_type_id")] [BsonRepresentation(BsonType.ObjectId)]
        public string ProfessorTypeId { get; set; }

        [BsonElement("course_id")] [BsonRepresentation(BsonType.ObjectId)]
        public string CourseId { get; set; }

        [BsonElement("professor_id")] [BsonRepresentation(BsonType.ObjectId)]
        public string ProfessorId { get; set; }
    }
}