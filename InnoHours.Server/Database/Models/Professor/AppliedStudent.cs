using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InnoHours.Server.Database.Models.Professor
{
    public class AppliedStudent
    {
        [BsonElement("_id")] [BsonRepresentation(BsonType.ObjectId)]

        public string StudentId { get; set; }

        [BsonElement("student_name")]
        public string StudentName { get; set; }
    }
}