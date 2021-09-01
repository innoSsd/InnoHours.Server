namespace InnoHours.Server.Models.Authentication
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public string UserId { get; set; }

        public string UserType { get; set; }

        public string UserFullName { get; set; }
    }
}