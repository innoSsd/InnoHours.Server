using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InnoHours.Server.DataBase.Models
{
    public class Professor
    {
        [BsonId] [BsonElement("_id")] [BsonRepresentation(BsonType.ObjectId)]
        public string Id {  get; set; }

        [BsonElement("email")]
        public string Email {  get; set; }

        [BsonElement("pass_hash")]
        public string PasswordHash { get; set; }

        [BsonElement("full_name")]
        public string FullName { get; set; }

        [BsonElement("assigned_groups")]
        public IList<string> AssignedGroups { get; set; }
    }
}