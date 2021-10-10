using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InnoHours.Server.Database.Models.Professor
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
        public IList<AssignedGroup> AssignedGroups { get; set; }

        [BsonElement("office_hours")]
        public IList<OfficeHours> OfficeHours { get; set; }

        [BsonElement("office_hours_requests")]
        public IList<OfficeHoursRequest> OfficeHoursRequests { get; set; }

        [BsonElement("allow_requests")]
        public bool AllowRequests { get; set; }

        [BsonElement("can_open_office_hours")]
        public bool CanOpenOfficeHours { get; set; }

    }
}