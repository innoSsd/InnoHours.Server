using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InnoHours.Server.Database.Models.Student
{
    public class OfficeHoursRequest
    {
        [BsonElement("professor_id")] [BsonRepresentation(BsonType.ObjectId)]
        public string ProfessorId { get; set; }

        [BsonElement("status")]
        public string Status {  get; set; }

        [BsonElement("request_id")] [BsonRepresentation(BsonType.ObjectId)]
        public string RequestId { get; set; }

        [BsonElement("professor_name")]
        public string ProfessorName { get; set; }
    }
}