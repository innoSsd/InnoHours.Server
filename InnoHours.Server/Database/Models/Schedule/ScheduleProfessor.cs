using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InnoHours.Server.Database.Models.Schedule
{
    public class ScheduleProfessor
    {
        [BsonElement("_id")] [BsonRepresentation(BsonType.ObjectId)]
        public string ProfessorId { get; set; }

        [BsonElement("professor_name")]
        public string ProfessorName {  get; set; }
    }
}