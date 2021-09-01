using System.Threading.Tasks;
using InnoHours.Server.DataBase;
using InnoHours.Server.DataBase.Models;

namespace InnoHours.Server.Authentication
{
    public class UserAuthenticator : IUserAuthenticator
    {
        private readonly UsersDbContext _context;

        public UserAuthenticator(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<Student> AuthenticateStudentAsync(string email, string passwordHash)
        {
            var student = await _context.FindStudentByEmailAsync(email);
            if (student == null) return null;
            return student.PasswordHash == passwordHash ? student : null;
        }

        public async Task<Professor> AuthenticateProfessorAsync(string email, string passwordHash)
        {
            var professor = await _context.FindProfessorByEmailAsync(email);
            if (professor == null) return null;
            return professor.PasswordHash == passwordHash ? professor : null;
        }

        public async Task<Student> GetStudentByIdAsync(string id)
        {
            var user = await _context.FindStudentByIdAsync(id);
            return user;
        }

        public async Task<Professor> GetProfessorByIdAsync(string id)
        {
            var user = await _context.FindProfessorByIdAsync(id);
            return user;
        }
    }
}