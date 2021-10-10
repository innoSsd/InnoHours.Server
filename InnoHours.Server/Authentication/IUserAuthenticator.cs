using System.Threading.Tasks;
using InnoHours.Server.Database.Models.Professor;
using InnoHours.Server.Database.Models.Student;

namespace InnoHours.Server.Authentication
{
    public interface IUserAuthenticator
    {
        Task<Student> AuthenticateStudentAsync(string email, string passwordHash);

        Task<Professor> AuthenticateProfessorAsync(string email, string passwordHash);

        Task<Student> GetStudentByIdAsync(string id);

        Task<Professor> GetProfessorByIdAsync(string id);
    }
}