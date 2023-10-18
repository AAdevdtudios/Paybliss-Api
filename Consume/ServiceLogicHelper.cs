using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Paybliss.Models;
using Paybliss.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Paybliss.Consume
{
    public class ServiceLogicHelper : IServiceLogicHelper
    {
        private readonly IConfiguration _configuration;
        private readonly JWTSettings _jWTSettings;
        public ServiceLogicHelper(IConfiguration configuration, IOptions<JWTSettings> options)
        {
            _configuration = configuration;
            _jWTSettings = options.Value;
        }
        public string CreateJWToken(User user)  
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_jWTSettings.Secret);
            var tokenDesc = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Hash, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                Issuer = _jWTSettings.Issuer,
                Audience = _jWTSettings.Audience,
                SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDesc);
            return tokenHandler.WriteToken(token);
        }

        public void CreatePasswordHash(string Password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
            }
        }

        public string CreateToken()
        {
            Random random = new Random();
            string token = random.Next(1000,9999).ToString();
            return token;
        }

        public bool VerifyPasswordHash(string Password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        public string GenerateRefreshToken()
        {
            return RandomString(25) + Guid.NewGuid().ToString();
        }
        public string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
