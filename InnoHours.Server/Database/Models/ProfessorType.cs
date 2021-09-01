using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InnoHours.Server.DataBase.Models
{
    public class ProfessorType
    {
        [BsonId] [BsonElement("type_id")] [BsonRepresentation(BsonType.ObjectId)]
        public string TypeId { get; set; }

        [BsonElement("type_name")]
        public string TypeName {  get; set; }
    }
}