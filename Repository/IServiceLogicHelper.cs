using Paybliss.Models;

namespace Paybliss.Repository
{
    public interface IServiceLogicHelper
    {
        void CreatePasswordHash(string Password, out byte[] passwordHash, out byte[] passwordSalt);
        string CreateToken();
        bool VerifyPasswordHash(string Password, byte[] passwordHash, byte[] passwordSalt);
        string CreateJWToken(User user);
        string GenerateRefreshToken();
        //Task SendEmail(string email, string otp);
    }
}
