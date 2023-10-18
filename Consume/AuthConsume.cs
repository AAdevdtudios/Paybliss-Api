using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Paybliss.Data;
using Paybliss.Models;
using Paybliss.Models.Dto;
using Paybliss.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Paybliss.Consume
{
    public class AuthConsume : IAuthRepo
    {
        private readonly DataContext _context;
        private readonly IServiceLogicHelper _passwordHash;
        private readonly IMapper _mapper;
        private readonly JWTSettings _jWTSettings;

        public AuthConsume(DataContext context, IServiceLogicHelper passwordHash, IMapper mapper, IOptions<JWTSettings> options)
        {
            _context = context;
            _passwordHash = passwordHash;
            _mapper = mapper;
            _jWTSettings = options.Value;
        }

        public async Task<ResponseData<UserDto>> LoginUser(LoginDto loginDto)
        {
            var user = await _context.User.FirstOrDefaultAsync(o => o.Email == loginDto.Email);
            var refreshToken = new RefreshToken();
            ResponseData<UserDto> response = new ResponseData<UserDto>();
            if (user == null)
            {
                response.Message = "User doesn't exist";
                response.Successful = false;
                response.StatusCode = 400;
                return response;
            }

            if(!_passwordHash.VerifyPasswordHash(loginDto.Password,user.passwordHash, user.passwordSalt))
            {
                response.Message = "User password is not correct";
                response.Successful = false;
                response.StatusCode = 400;
                return response;
            }

            /*if (user.VerifiedAt == null)
            {
                response.Message = "User is not verified";
                response.Successful = false;
                response.StatusCode = 400;
                return response;
            }*/
            refreshToken.Token = ConfirmTokens();
            refreshToken.user = user;
            var token = _context.RefreshTokens.FirstOrDefault(o => o.UserId == user.Id);

            if (token == null)
            {
                _context.RefreshTokens.Add(refreshToken);
            }
            else
            {
                token.Token = refreshToken.Token;
            }
            
            await _context.SaveChangesAsync();
            var userDto = _mapper.Map<UserDto>(user);

            userDto.RefreshToken = refreshToken.Token;
            userDto.JWToken = _passwordHash.CreateJWToken(user);

            response.Message = $"Welcome back {userDto.Email}";
            response.Successful = true;
            response.StatusCode = 200;
            response.Data = userDto;

            return response;
        }
        private string ConfirmTokens()
        {
            if(_context.RefreshTokens.FirstOrDefault(o=> o.Token == _passwordHash.GenerateRefreshToken()) != null)
            {
                return ConfirmTokens();
            }
            return _passwordHash.GenerateRefreshToken();
        }

        public async Task<ResponseData<UserDto>> RegisterUser(RegisterDto registerDto)
        {
            var response = new ResponseData<UserDto>();
            if(_context.User.FirstOrDefault(o => o.Email == registerDto.Email) != null)
            {
                response.Successful = false;
                response.Message = "User already exist";
                response.StatusCode = 400;
                return response;
            }
            _passwordHash.CreatePasswordHash(registerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            registerDto.VerificationToken = _passwordHash.CreateToken();

            var userData = _mapper.Map<User>(registerDto);
            userData.passwordHash = passwordHash;
            userData.passwordSalt = passwordSalt;
            _context.User.Add(userData);
            await _context.SaveChangesAsync();

            response.Successful = true;
            response.Message = $"User with email: {userData.Email} has been created successfully";
            response.StatusCode = 200;
            response.Data = _mapper.Map<UserDto>(userData);
            return response;
        }

        public async Task<ResponseData<UserDto>> VerifyUser(VerifyDto verifyDto)
        {
            var user = await _context.User.FirstOrDefaultAsync(o => o.Email == verifyDto.Email);
            var response = new ResponseData<UserDto>();

            if (user == null)
            {
                response.Successful = false;
                response.Message = "User doesn't exist";
                response.StatusCode = 400;
                return response;
            }
            if(user.VerificationToken != verifyDto.token)
            {
                response.Successful = false;
                response.Message = "Invalid token address";
                response.StatusCode = 400;
                return response;
            }
            if(user.VerificationToken == "")
            {
                response.Successful = false;
                response.Message = "User already verified";
                response.StatusCode = 400;
                return response;
            }
            user.VerifiedAt = DateTime.Now;
            user.VerificationToken = "";
            await _context.SaveChangesAsync();
            response.Successful = true;
            response.StatusCode = 200;
            response.Message = "User is verified";
            response.Data = _mapper.Map<UserDto>(user);
            return response;
        }

        public async Task<ResponseData<string>> SendToken(string email)
        {
            var user = await _context.User.FirstOrDefaultAsync(o => o.Email == email);
            var response = new ResponseData<string>();
            if (user == null)
            {
                response.Successful = false;
                response.Message = "User doesn't exist";
                response.StatusCode = 400;
                return response;
            }
            user.PasswordReset = _passwordHash.CreateToken();
            user.ResetTokenExpires = DateTime.Today;
            await _context.SaveChangesAsync();

            response.Successful = true;
            response.Message = $"Password rest token sent: {user.PasswordReset}";
            response.StatusCode = 200;
            return response;
        }

        public async Task<ResponseData<UserDto>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _context.User.FirstOrDefaultAsync(o => o.Email == resetPasswordDto.Email);
            var response = new ResponseData<UserDto>();

            if (user == null)
            {
                response.Successful = false;
                response.Message = "User doesn't exist";
                response.StatusCode = 400;

                return response;
            }
            if (user.ResetTokenExpires != DateTime.Today)
            {
                response.Successful = false;
                response.Message = "Token has expired";
                response.StatusCode = 400;

                return response;
            }

            if (user.PasswordReset != resetPasswordDto.ResetToken || user.PasswordReset == "")
            {
                response.Successful = false;
                response.StatusCode = 400;
                response.Message = "Token is not correct";
                return response;
            }
            _passwordHash.CreatePasswordHash(resetPasswordDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.passwordHash = passwordHash;
            user.passwordSalt = passwordSalt;
            user.PasswordReset = "";

            await _context.SaveChangesAsync();


            response.Successful = true;
            response.StatusCode = 200;
            response.Message = "Password reset successful";
            response.Data = _mapper.Map<UserDto>(user);

            return response;
        }

        public async Task<ResponseData<RefreshTokenDto>> RefreshToken(RefreshTokenDto refreshToken)
        {
            var _refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(o=>o.Token == refreshToken.refresh_token);
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            ResponseData<RefreshTokenDto> response = new ResponseData<RefreshTokenDto>();


            if (_refreshToken == null)
            {
                response.Successful = false;
                response.Message = "Un-authorized";
                response.StatusCode = 400;
                return response;
            }

            var validatedToken = jwtTokenHandler.ReadJwtToken(refreshToken.token);

            if (validatedToken is JwtSecurityToken securityToken)
            {
                var result = securityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                if (result == false)
                {
                    response.Successful = false;
                    response.Message = "Token is not valid";
                    response.StatusCode = 498;
                    return response;
                }
                var utcExpireDate = long.Parse(validatedToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)!.Value);
                var expDate = UnixTimeStampToDateTime(utcExpireDate);
                if (expDate > DateTime.UtcNow)
                {
                    response.Successful = false;
                    response.Message = "Token has not expired";
                    response.StatusCode = 498;
                    return response;
                }
            }

            var user = await _context.User.FirstOrDefaultAsync(db => db.Id == _refreshToken!.UserId);
            _refreshToken!.Token = _passwordHash.GenerateRefreshToken();
            await _context.SaveChangesAsync();


            refreshToken.token = _passwordHash.CreateJWToken(user!);
            refreshToken.refresh_token = _refreshToken.Token;

            response.Successful = true;
            response.Message = "New token generated";
            response.StatusCode = 200;
            response.Data = refreshToken;

            return response;
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0,System.DateTimeKind.Utc);
            return dateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
        }

        public async Task<ResponseData<UserDto>> SetPin(SetPinDto setPin)
        {
            var user = _context.User.FirstOrDefault(o=>o.Email == setPin.email);
            var response = new ResponseData<UserDto>();
            if(user == null)
            {
                response.Successful = false;
                response.Message = "User already exist";
                response.StatusCode = 400;
                return response;
            }
            user.Pin = setPin.pin;
            await _context.SaveChangesAsync();
            response.Successful = true;
            response.Message = "Pin set successfully";
            response.Data = _mapper.Map<UserDto>(user);

            return response;
        }

        public async Task<ResponseData<UserDto>> GetUser(string email, int pin)
        {
            var response = new ResponseData<UserDto>();
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(o => o.Email == email);
                if(user!.Pin != pin)
                {
                    response.Successful = false;
                    response.Message = "Pin doesn't match";
                    response.StatusCode = 400;
                    return response;
                }
                response.Data = _mapper.Map<UserDto>(user);
                response.Successful = true;
                response.StatusCode = 200;
                response.Message = "User";
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.StatusCode = 400;
                response.Message = e.Message;
            }

            return response;
        }
        public async Task<ResponseData<UserDto>> UpdateUser(string email, UserDto userData)
        {
            var response = new ResponseData<UserDto>();
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(o => o.Email == email);
                var newEmail = await _context.User.FirstOrDefaultAsync(o => o.Email == userData.Email);
                if(newEmail != null)
                {
                    response.Message = "Email is already in use";
                    response.Successful = false;
                    response.StatusCode = 409;
                    return response;
                }
                user!.Email = userData.Email;
                user.FirstName = userData.FirstName;
                user.LastName = userData.LastName;
                await _context.SaveChangesAsync();
                response.Successful = true;
                response.Data = userData;
                response.Message = "User data updated";
                response.Data = _mapper.Map<UserDto>(user);
                return response;
            }
            catch (Exception e)
            {
                response.Successful = false;
                response.StatusCode = 400;
                response.Message = e.Message;
                return response;
            }
        }

    }
}
 