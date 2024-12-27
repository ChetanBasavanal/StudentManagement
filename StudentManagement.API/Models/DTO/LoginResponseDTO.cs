using System.Security.Principal;

namespace StudentManagement.API.Models.DTO
{
    public class LoginResponseDTO
    {
        public string JwtToken { get; set; }
    }
}
