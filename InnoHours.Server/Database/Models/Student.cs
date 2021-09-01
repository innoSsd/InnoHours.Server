using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InnoHours.Server.DataBase.Models
{
    public class Student
    {
        [BsonId] [BsonElement("_id")] [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("email")]
        public string Email {  get; set; }

        [BsonElement("full_name")] public string FullName { get; set; }

        [BsonElement("pass_hash")] public string PasswordHash { get; set; }

        [BsonElement("group_id")] [BsonRepresentation(BsonType.ObjectId)]
        public string GroupId { get; set; }

    }
}