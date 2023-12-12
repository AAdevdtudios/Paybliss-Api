using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Paybliss.Models.DataResponse
{
    public class UserItems: User
    {
        public string JWToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
