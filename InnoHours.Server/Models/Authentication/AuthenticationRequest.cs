namespace InnoHours.Server.Models.Authentication
{
    public class AuthenticationRequest
    {
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Platform { get; set; }
    }
}