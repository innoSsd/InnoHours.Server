using System.Threading.Tasks;
using InnoHours.Server.DataBase.Models;

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