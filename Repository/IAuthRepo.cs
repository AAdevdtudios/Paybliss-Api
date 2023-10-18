using Paybliss.Models;
using Paybliss.Models.Dto;

namespace Paybliss.Repository
{
    public interface IAuthRepo
    {
        Task<ResponseData<UserDto>> RegisterUser(RegisterDto registerDto);
        Task<ResponseData<UserDto>> LoginUser(LoginDto loginDto);
        Task<ResponseData<UserDto>> VerifyUser(VerifyDto verifyDto);
        Task<ResponseData<string>> SendToken(string email);
        Task<ResponseData<UserDto>> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<ResponseData<RefreshTokenDto>> RefreshToken(RefreshTokenDto refreshToken);
        Task<ResponseData<UserDto>> SetPin(SetPinDto setpin);
        Task<ResponseData<UserDto>> GetUser(string user, int pin);
        Task<ResponseData<UserDto>> UpdateUser(string email, UpdateUserDto user);
    }
}
