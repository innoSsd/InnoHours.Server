using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InnoHours.Server.Database.Models.Student
{
    public class Student
    {
        [BsonId] [BsonElement("_id")] [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("email")]
        public string Email {  get; set; }

        [BsonElement("full_name")] public string FullName { get; set; }

        [BsonElement("pass_hash")] public string PasswordHash { get; set; }

        [BsonElement("grade")]
        public string Grade { get; set; }

        [BsonElement("group")]
        public string Group {  get; set; }

        [BsonElement("applied_office_hours")]
        public IList<AppliedOfficeHours> AppliedOfficeHours { get; set; }

        [BsonElement("requested_office_hours")]
        public IList<OfficeHoursRequest> OfficeHoursRequests { get; set; }

    }
}