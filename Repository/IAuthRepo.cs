using Paybliss.Models;
using Paybliss.Models.DataResponse;
using Paybliss.Models.Dto;

namespace Paybliss.Repository
{
    public interface IAuthRepo
    {
        Task<ResponseData<UserItems>> RegisterUser(RegisterDto registerDto);
        Task<ResponseData<UserItems>> LoginUser(LoginDto loginDto);
        Task<ResponseData<UserItems>> VerifyUser(VerifyDto verifyDto);
        Task<ResponseData<string>> SendToken(string email);
        Task<ResponseData<UserItems>> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<ResponseData<RefreshTokenDto>> RefreshToken(RefreshTokenDto refreshToken);
        Task<ResponseData<UserItems>> SetPin(SetPinDto setpin);
        Task<ResponseData<UserItems>> GetUser(string user, int pin);
        Task<ResponseData<UserItems>> UpdateUser(string email, UpdateUserDto user);
        Task<ResponseData<UserItems>> UpdatePassword(string email, UpdatePasswordDto passwordDto);
        Task<bool> VerifyBvn(string bvn, string emaill);
    }
}
