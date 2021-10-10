using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InnoHours.Server.Database.Models.Student
{
    public class AppliedOfficeHours
    {
        [BsonElement("office_hours_id")] [BsonRepresentation(BsonType.ObjectId)]
        public string OfficeHoursId { get; set; }

        [BsonElement("professor_id")] [BsonRepresentation(BsonType.ObjectId)]
        public string ProfessorId { get; set; }
    }
}