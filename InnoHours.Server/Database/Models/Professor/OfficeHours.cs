using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InnoHours.Server.Database.Models.Professor
{
    public class OfficeHours
    {
        [BsonRepresentation(BsonType.ObjectId)] [BsonElement("_id")]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title {  get; set; }

        [BsonElement("description")]
        public string Description {  get; set; }

        [BsonElement("grade")]
        public string Grade { get; set; }

        [BsonElement("group")]
        public string Group {  get; set; }

        [BsonElement("students_limit")]
        public int StudentsLimit { get; set; }

        [BsonElement("date")]
        public DateTime Date {  get; set; }

        [BsonElement("time_start")]
        public string TimeStart { get; set; }

        [BsonElement("time_end")]
        public string TimeEnd {  get; set; }

        [BsonElement("course_name")]
        public string CourseName { get; set; }

        [BsonElement("applied_students")]
        public IList<AppliedStudent> AppliedStudents { get; set; }

        [BsonElement("location")]
        public string Location {  get; set; }

        [BsonElement("repeat_every")]
        public string RepeatEvery { get; set; }
    }
}