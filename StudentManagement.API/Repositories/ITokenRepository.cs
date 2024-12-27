using Microsoft.AspNetCore.Identity;

namespace StudentManagement.API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
