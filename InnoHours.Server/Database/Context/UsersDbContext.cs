using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using InnoHours.Server.Database.Models.Professor;
using InnoHours.Server.Database.Models.Student;
using MongoDB.Driver;

namespace InnoHours.Server.Database.Context
{
    public class UsersDbContext
    {
        public UsersDbContext(MainDbContext context)
        {
            _studentsCollection = context.MainDatabase.GetCollection<Student>("students");
            _professorCollection = context.MainDatabase.GetCollection<Professor>("professors");
        }

        private readonly IMongoCollection<Student> _studentsCollection;
        private readonly IMongoCollection<Professor> _professorCollection;

        public async Task<Student> FindStudentByEmailAsync(string email)
        {
            var foundStudentCursor = await _studentsCollection.FindAsync(student => student.Email == email);
            var foundStudentsList = await foundStudentCursor.ToListAsync();
            ValidateUsersCount(foundStudentsList, "email");
            return foundStudentsList.Count == 0 ? null : foundStudentsList[0];
        }

        public async Task<Professor> FindProfessorByEmailAsync(string email)
        {
            var foundProfessorCursor = await _professorCollection.FindAsync(professor => professor.Email == email);
            var foundProfessorsList = await foundProfessorCursor.ToListAsync();
            ValidateUsersCount(foundProfessorsList, "email");
            return foundProfessorsList.Count == 0 ? null : foundProfessorsList[0];
        }
        
        public async Task<Student> FindStudentByIdAsync(string id)
        {
            var foundStudentCursor = await _studentsCollection.FindAsync(student => student.Id == id);
            var foundStudentsList = await foundStudentCursor.ToListAsync();
            return foundStudentsList.Count == 0 ? null : foundStudentsList[0];
        }

        public async Task<Professor> FindProfessorByIdAsync(string id)
        {
            var foundProfessorCursor = await _professorCollection.FindAsync(professor => professor.Id == id);
            var foundProfessorsList = await foundProfessorCursor.ToListAsync();
            return foundProfessorsList.Count == 0 ? null : foundProfessorsList[0];
        }

        private static void ValidateUsersCount(ICollection<Student> students, string propertyToVerify)
        {
            Debug.Assert(students.Count <= 1, $"Found duplicated {propertyToVerify}s in collection Students");
        }

        private static void ValidateUsersCount(ICollection<Professor> professors, string propertyToVerify)
        {
            Debug.Assert(professors.Count <= 1, $"Found duplicated {propertyToVerify}s in collection Professors");
        }

    }
}