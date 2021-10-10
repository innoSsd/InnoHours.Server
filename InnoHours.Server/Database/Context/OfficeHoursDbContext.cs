using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InnoHours.Server.Database.Data.OfficeHours;
using InnoHours.Server.Database.Models.Professor;
using InnoHours.Server.Database.Models.Student;
using MongoDB.Bson;
using MongoDB.Driver;
using StudentOfficeHoursRequest = InnoHours.Server.Database.Models.Student.OfficeHoursRequest;
using ProfessorOfficeHoursRequest = InnoHours.Server.Database.Models.Professor.OfficeHoursRequest;

namespace InnoHours.Server.Database.Context
{
    public class OfficeHoursDbContext
    {
        private readonly IMongoCollection<Professor> _professorsCollection;
        private readonly IMongoCollection<Student> _studentsCollection;
        private readonly MongoClient _client;

        public OfficeHoursDbContext(MainDbContext context)
        {
            _client = context.Client;
            _professorsCollection = context.MainDatabase.GetCollection<Professor>("professors");
            _studentsCollection = context.MainDatabase.GetCollection<Student>("students");
        }

        public async Task<string> CreateOfficeHoursAsync(string professorId, OfficeHoursCreating creatingData)
        {
            var newOfficeHoursId = new BsonObjectId(ObjectId.GenerateNewId()).Value.ToString();
            var updateFilter = Builders<Professor>.Update.Push(professor => professor.OfficeHours, new OfficeHours
            {
                AppliedStudents = new List<AppliedStudent>(),
                Grade = creatingData.Grade,
                Group = creatingData.Group,
                CourseName = creatingData.CourseName,
                Date = creatingData.Date,
                Description = creatingData.Description,
                Id = newOfficeHoursId,
                TimeStart = creatingData.TimeStart,
                TimeEnd = creatingData.TimeEnd,
                Location = creatingData.Location,
                RepeatEvery = creatingData.RepeatEvery,
                StudentsLimit = creatingData.StudentsLimit,
                Title = creatingData.Title
            });
            var updateResult =
                await _professorsCollection.UpdateOneAsync(professor => professor.Id == professorId, updateFilter);
            return !updateResult.IsAcknowledged ? null : newOfficeHoursId;
        }

        public async Task<string> CreateOfficeHoursRequest(string professorId, OfficeHoursRequestCreating creatingData)
        {
            var officeHoursRequestId = new BsonObjectId(ObjectId.GenerateNewId()).Value.ToString();
            var foundProfessor = await ValidateProfessorExistsAndGet(professorId);

            using var session = await _client.StartSessionAsync();
            session.StartTransaction();

            var studentUpdateFilter = Builders<Student>.Update.Push(student => student.OfficeHoursRequests,
                new StudentOfficeHoursRequest
                {
                    ProfessorId = professorId,
                    RequestId = officeHoursRequestId,
                    Status = "open",
                    ProfessorName = foundProfessor.FullName
                });
            var studentUpdateResult =
                await _studentsCollection.UpdateOneAsync(student => student.Id == creatingData.StudentId,
                    studentUpdateFilter);
            if (!studentUpdateResult.IsAcknowledged)
            {
                await session.AbortTransactionAsync();
                return null;
            }

            var professorUpdateFilter = Builders<Professor>.Update.Push(professor => professor.OfficeHoursRequests,
                new ProfessorOfficeHoursRequest
                {
                    RequestId = officeHoursRequestId,
                    StudentId = creatingData.StudentId,
                    StudentName = creatingData.StudentName
                });
            var professorUpdateResult =
                await _professorsCollection.UpdateOneAsync(professor => professor.Id == professorId,
                    professorUpdateFilter);
            if (!professorUpdateResult.IsAcknowledged)
            {
                await session.AbortTransactionAsync();
                return null;
            }

            await session.CommitTransactionAsync();
            return officeHoursRequestId;
        }

        public async Task<string> AcceptOfficeHoursRequestAsync(string professorId, string officeHoursRequestId,
            OfficeHoursCreating creatingData)
        {

            var professorCursor =
                await _professorsCollection.FindAsync(professor => professor.Id == professorId);
            var foundProfessor = await professorCursor.FirstOrDefaultAsync();

            var foundOfficeHours =
                foundProfessor?.OfficeHoursRequests.FirstOrDefault(request =>
                    request.RequestId == officeHoursRequestId);
            if (foundOfficeHours == null) return null;


            using var session = await _client.StartSessionAsync();
            session.StartTransaction();

            var newOfficeHoursId = new BsonObjectId(ObjectId.GenerateNewId()).Value.ToString();

            var studentUpdateFilter = Builders<Student>.Update
                .PullFilter(student => student.OfficeHoursRequests,
                    request => request.RequestId == officeHoursRequestId)
                .Push(student => student.AppliedOfficeHours, new AppliedOfficeHours
                {
                    ProfessorId = professorId,
                    OfficeHoursId = newOfficeHoursId
                });

            var studentUpdateResult =
                await _studentsCollection.UpdateOneAsync(student => student.Id == foundOfficeHours.StudentId,
                    studentUpdateFilter);
            if (!studentUpdateResult.IsAcknowledged)
            {
                await session.AbortTransactionAsync();
                return null;
            }

            var professorUpdateFilter = Builders<Professor>.Update
                .PullFilter(professor => professor.OfficeHoursRequests,
                    request => request.RequestId == officeHoursRequestId)
                .Push(professor => professor.OfficeHours, new OfficeHours
                {
                    Grade = creatingData.Grade,
                    AppliedStudents = new List<AppliedStudent>(new[]
                    {
                        new AppliedStudent
                        {
                            StudentId = foundOfficeHours.StudentId,
                            StudentName = foundOfficeHours.StudentName
                        }
                    }),
                    CourseName = creatingData.CourseName,
                    Date = creatingData.Date,
                    Description = creatingData.Description,
                    Group = creatingData.Group,
                    Id = newOfficeHoursId,
                    TimeEnd = creatingData.TimeEnd,
                    Location = creatingData.Location,
                    TimeStart = creatingData.TimeStart,
                    RepeatEvery = creatingData.RepeatEvery,
                    StudentsLimit = creatingData.StudentsLimit,
                    Title = creatingData.Title
                });
            var professorUpdateResult =
                await _professorsCollection.UpdateOneAsync(professor => professor.Id == professorId,
                    professorUpdateFilter);

            if (!professorUpdateResult.IsAcknowledged)
            {
                await session.AbortTransactionAsync();
                return null;
            }

            await session.CommitTransactionAsync();
            return newOfficeHoursId;
        }

        public async Task<bool> DeclineOfficeHoursRequestAsync(string professorId, string officeHoursRequestId)
        {
            var professorCursor =
                await _professorsCollection.FindAsync(professor => professor.Id == professorId);
            var foundProfessor = await professorCursor.FirstOrDefaultAsync();

            var foundOfficeHours =
                foundProfessor?.OfficeHoursRequests.FirstOrDefault(request =>
                    request.RequestId == officeHoursRequestId);
            if (foundOfficeHours == null) return false;

            using var session = await _client.StartSessionAsync();
            session.StartTransaction();

            var studentUpdateFilter =
                Builders<Student>.Update.Set(student => student.OfficeHoursRequests[-1].Status, "declined");
            var studentFilter = Builders<Student>.Filter.Where(student => student.Id == foundOfficeHours.StudentId) &
                                Builders<Student>.Filter.ElemMatch(student => student.OfficeHoursRequests,
                                    request => request.RequestId == officeHoursRequestId);
            var studentUpdateResult = await _studentsCollection.UpdateOneAsync(studentFilter, studentUpdateFilter);
            if (!studentUpdateResult.IsAcknowledged)
            {
                await session.AbortTransactionAsync();
                return false;
            }

            var professorUpdateFilter = Builders<Professor>.Update.PullFilter(
                professor => professor.OfficeHoursRequests, request => request.RequestId == officeHoursRequestId);
            var professorUpdateResult =
                await _professorsCollection.UpdateOneAsync(professor => professor.Id == professorId,
                    professorUpdateFilter);
            if (!professorUpdateResult.IsAcknowledged)
            {
                await session.AbortTransactionAsync();
                return false;
            }

            await session.CommitTransactionAsync();
            return true;
        }

        public async Task<bool> StudentApplyForOfficeHours(string professorId, string officeHoursId, string studentId)
        {
            var studentCursor = await _studentsCollection.FindAsync(student => student.Id == studentId);
            var student = await studentCursor.FirstOrDefaultAsync();
            if (student == null) return false;

            var professorCursor = await _professorsCollection.FindAsync(professor => professor.Id == professorId);
            var professor = await professorCursor.FirstOrDefaultAsync();
            var officeHours = professor?.OfficeHours?.FirstOrDefault(hours => hours.Id == officeHoursId);
            if (officeHours == null) return false;

            if (officeHours.StudentsLimit <= officeHours.AppliedStudents.Count)
                return false;

            using var session = await _client.StartSessionAsync();
            session.StartTransaction();

            var studentUpdateFilter = Builders<Student>.Update.Push(student1 => student1.AppliedOfficeHours,
                new AppliedOfficeHours
                {
                    ProfessorId = professorId,
                    OfficeHoursId = officeHoursId
                });
            var studentUpdateResult =
                await _studentsCollection.UpdateOneAsync(student1 => student1.Id == studentId, studentUpdateFilter);
            if (!studentUpdateResult.IsAcknowledged)
            {
                await session.AbortTransactionAsync();
                return false;
            }

            var professorUpdateFilter = Builders<Professor>.Update.Push(
                professor1 => professor1.OfficeHours[-1].AppliedStudents, new AppliedStudent
                {
                    StudentId = studentId,
                    StudentName = student.FullName
                });
            var professorFilter = Builders<Professor>.Filter.Where(professor1 => professor1.Id == professorId) &
                                  Builders<Professor>.Filter.ElemMatch(professor1 => professor1.OfficeHours,
                                      hours => hours.Id == officeHoursId);
            var professorUpdateResult =
                await _professorsCollection.UpdateOneAsync(professorFilter, professorUpdateFilter);
            if (!professorUpdateResult.IsAcknowledged)
            {
                await session.AbortTransactionAsync();
                return false;
            }

            await session.CommitTransactionAsync();
            return true;
        }

        public async Task<bool> CancelStudentApplyForOfficeHoursAsync(string professorId, string officeHoursId,
            string studentId)
        {
            var studentCursor = await _studentsCollection.FindAsync(student => student.Id == studentId);
            var student = await studentCursor.FirstOrDefaultAsync();
            if (student == null) return false;

            var professorCursor = await _professorsCollection.FindAsync(professor => professor.Id == professorId);
            var professor = await professorCursor.FirstOrDefaultAsync();
            var officeHours = professor?.OfficeHours?.FirstOrDefault(hours => hours.Id == officeHoursId);
            if (officeHours == null) return false;

            using var session = await _client.StartSessionAsync();
            session.StartTransaction();

            var studentUpdateFilter = Builders<Student>.Update.PullFilter(student1 => student1.AppliedOfficeHours,
                hours => hours.OfficeHoursId == officeHoursId);
            var studentUpdateResult =
                await _studentsCollection.UpdateOneAsync(student1 => student1.Id == studentId, studentUpdateFilter);
            if (!studentUpdateResult.IsAcknowledged)
            {
                await session.AbortTransactionAsync();
                return false;
            }

            var professorUpdateFilter = Builders<Professor>.Update.PullFilter(
                professor1 => professor1.OfficeHours[-1].AppliedStudents,
                appliedStudent => appliedStudent.StudentId == studentId);
            var professorFilter = Builders<Professor>.Filter.Where(professor1 => professor1.Id == professorId) &
                                  Builders<Professor>.Filter.ElemMatch(professor1 => professor1.OfficeHours,
                                      hours => hours.Id == officeHoursId);
            var professorUpdateResult =
                await _professorsCollection.UpdateOneAsync(professorFilter, professorUpdateFilter);
            if (!professorUpdateResult.IsAcknowledged)
            {
                await session.AbortTransactionAsync();
                return false;
            }

            await session.CommitTransactionAsync();
            return true;
        }

        public async Task<bool> DeleteOfficeHours(string professorId, string officeHoursId)
        {
            var professorCursor = await _professorsCollection.FindAsync(professor => professor.Id == professorId);
            var professor = await professorCursor.FirstOrDefaultAsync();
            var officeHours = professor?.OfficeHours?.FirstOrDefault(hours => hours.Id == officeHoursId);
            if (officeHours == null) return false;

            using var session = await _client.StartSessionAsync();
            session.StartTransaction();

            var studentUpdateFilter = Builders<Student>.Update.PullFilter(student => student.AppliedOfficeHours,
                hours => hours.OfficeHoursId == officeHoursId);
            await _studentsCollection.UpdateOneAsync(FilterDefinition<Student>.Empty, studentUpdateFilter);

            var professorUpdateFilter = Builders<Professor>.Update.PullFilter(professor1 => professor1.OfficeHours,
                hours => hours.Id == officeHoursId);
            var professorUpdateResult =
                await _professorsCollection.UpdateOneAsync(professor1 => professor1.Id == professorId,
                    professorUpdateFilter);
            if (!professorUpdateResult.IsAcknowledged)
            {
                await session.AbortTransactionAsync();
                return false;
            }

            await session.CommitTransactionAsync();
            return true;
        }

        public async Task<IEnumerable<OfficeHoursForStudent>> GetOfficeHoursForStudentAsync(string studentId)
        {
            var student = await ValidateStudentExistsAndGet(studentId);

            var professorWithOfficeHoursFilter = Builders<Professor>.Filter.ElemMatch(
                professor => professor.OfficeHours,
                hours => hours.Grade == student.Grade && (hours.Group == "" || hours.Group == student.Group));
            var professorCursor = await _professorsCollection.FindAsync(professorWithOfficeHoursFilter);
            var professorsWithOfficeHours = await professorCursor.ToListAsync();

            var officeHours = new List<OfficeHoursForStudent>();
            foreach (var professor in professorsWithOfficeHours)
            {
                officeHours.AddRange(
                    from officeHour in professor.OfficeHours
                    where officeHour.Grade == student.Grade &&
                          (officeHour.Group == "" || officeHour.Group == student.Group)
                    let officeHoursDate = officeHour.RepeatEvery switch
                    {
                        "week" => officeHour.Date.AddDays(GetDateWeekOffset(officeHour.Date) * 7),
                        "month" => officeHour.Date.AddMonths(GetDateMonthOffset(officeHour.Date)),
                        _ => officeHour.Date
                    }
                    let isStudentApplied = student.AppliedOfficeHours.Any(hours => hours.OfficeHoursId == officeHour.Id)
                    select new OfficeHoursForStudent
                    {
                        CourseName = officeHour.CourseName,
                        AppliedStudentsCount = officeHour.AppliedStudents.Count,
                        Date = officeHoursDate,
                        Description = officeHour.Description,
                        EndTime = officeHour.TimeEnd,
                        Grade = officeHour.Grade,
                        Group = officeHour.Group,
                        Id = officeHour.Id,
                        ProfessorId = professor.Id,
                        Location = officeHour.Location,
                        ProfessorName = professor.FullName,
                        StartTime = officeHour.TimeStart,
                        StudentsLimit = officeHour.StudentsLimit,
                        Title = officeHour.Title,
                        IsApplied = isStudentApplied
                    }
                );
            }

            return officeHours;
        }

        public async Task<IEnumerable<OfficeHoursForProfessor>> GetOfficeHoursForProfessorAsync(string professorId)
        {
            var professor = await ValidateProfessorExistsAndGet(professorId);


            return professor.OfficeHours.Select(hours =>
            {
                var courseDate = hours.RepeatEvery switch
                {
                    "week" => hours.Date.AddDays(GetDateWeekOffset(hours.Date) * 7),
                    "month" => hours.Date.AddMonths(GetDateMonthOffset(hours.Date)),
                    _ => hours.Date
                };
                return new OfficeHoursForProfessor
                {
                    AppliedStudents = hours.AppliedStudents,
                    CourseName = hours.CourseName,
                    Date = courseDate,
                    Description = hours.Description,
                    EndTime = hours.TimeEnd,
                    Grade = hours.Grade,
                    Group = hours.Group,
                    Id = hours.Id,
                    Location = hours.Location,
                    RepeatEvery = hours.RepeatEvery,
                    StartTime = hours.TimeStart,
                    StudentsLimit = hours.StudentsLimit,
                    Title = hours.Title
                };
            });
        }

        public async Task<IEnumerable<OfficeHoursStudentRequest>> GetStudentOfficeHoursRequests(string studentId)
        {
            var student = await ValidateStudentExistsAndGet(studentId);

            return student.OfficeHoursRequests.Select(request => new OfficeHoursStudentRequest
            {
                ProfessorId = request.ProfessorId,
                RequestId = request.RequestId,
                Status = request.Status
            });
        }

        public async Task<IEnumerable<OfficeHoursProfessorRequest>> GetProfessorOfficeHoursRequests(string professorId)
        {
            var professor = await ValidateProfessorExistsAndGet(professorId);

            return professor.OfficeHoursRequests.Select(request => new OfficeHoursProfessorRequest
            {
                RequestId = request.RequestId,
                StudentId = request.StudentId,
                StudentName = request.StudentName
            });
        }

        private async Task<Student> ValidateStudentExistsAndGet(string studentId)
        {
            var studentCursor = await _studentsCollection.FindAsync(student => student.Id == studentId);
            var student = await studentCursor.FirstOrDefaultAsync();
            if (student == null)
                throw new ArgumentException($"Student with id {studentId} not found");
            return student;
        }

        private async Task<Professor> ValidateProfessorExistsAndGet(string professorId)
        {
            var professorCursor = await _professorsCollection.FindAsync(professor => professor.Id == professorId);
            var professor = await professorCursor.FirstOrDefaultAsync();
            if (professor == null)
                throw new ArgumentException($"Professor with id {professorId} not found");
            return professor;
        }

        private static int GetDateWeekOffset(DateTime date)
        {
            var dateNow = DateTime.UtcNow.AddHours(3);

            if (dateNow <= date) return 0;
            
            var daysOffset = (dateNow - date).TotalDays;
            return (int)Math.Ceiling(daysOffset / 7.0);
        }

        private static int GetDateMonthOffset(DateTime date)
        {
            var dateNow = DateTime.UtcNow.AddHours(3);

            if (dateNow <= date) return 0;

            if (dateNow.Day < date.Day)
                return dateNow.Month - date.Month + (dateNow.Year - date.Year) * 12;
            return dateNow.Month - date.Month + (dateNow.Year - date.Year) * 12 + 1;
        }
    }
}