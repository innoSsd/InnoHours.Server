using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InnoHours.Server.Database.Models.Schedule
{
    public class GroupType
    {
        [BsonId] [BsonElement("_id")] [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("grade")]
        public string Grade { get; set; }

        [BsonElement("group")]
        public string Group {  get; set; }
    }
}