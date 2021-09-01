using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InnoHours.Server.DataBase.Models
{
    public class Group
    {
        [BsonId] [BsonElement("group_id")] [BsonRepresentation(BsonType.ObjectId)]
        public string GroupId { get; set; }

        [BsonElement("grade")]
        public string Grade { get; set; }

        [BsonElement("group")]
        public string GroupName { get; set; }
    }
}