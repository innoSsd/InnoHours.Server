using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InnoHours.Server.Database.Models.Schedule
{
    public class Schedule
    {
        [BsonId] [BsonRepresentation(BsonType.ObjectId)] [BsonElement("_id")]
        public string Id { get; set; }

        [BsonElement("day")]
        public string Day { get; set; }

        [BsonElement("time_start")]
        public string TimeStart { get; set; }

        [BsonElement("time_end")]
        public string TimeEnd {  get; set; }

        [BsonElement("grade")]
        public string Grade { get; set; }

        [BsonElement("group")]
        public string Group { get; set; }

        [BsonElement("course")]
        public ScheduleCourse Course { get; set; }

        [BsonElement("professor")]
        public ScheduleProfessor Professor { get; set; }

        [BsonElement("location")]
        public string Location {  get; set; }
    }
}