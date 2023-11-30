﻿using AutoMapper;
using Flurl.Http;
using Hangfire;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Paybliss.Data;
using Paybliss.Models;
using Paybliss.Models.Dto;
using Paybliss.Models.HttpResp;
using Paybliss.Repository;
using Paybliss.Repository.ServicesRepo;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Paybliss.Consume
{
    public class AuthConsume : IAuthRepo
    {
        private readonly DataContext _context;
        private readonly IServiceLogicHelper _passwordHash;
        private readonly IMapper _mapper;
        private readonly IBLOCService _bLOCService;
        private readonly JWTSettings _jWTSettings;
        public static string url = Environment.GetEnvironmentVariable("FLUTTERWAVEURL");
        public static string flutterwaveSK = Environment.GetEnvironmentVariable("FLUTTERWAVESK");
        private readonly string apiUrl = Environment.GetEnvironmentVariable("BLOCURL");
        private readonly string apiKey = Environment.GetEnvironmentVariable("BLOCKEY");

        public AuthConsume(DataContext context, IServiceLogicHelper passwordHash, IMapper mapper, IBLOCService bLOCService, IOptions<JWTSettings> options)
        {
            _context = context;
            _passwordHash = passwordHash;
            _mapper = mapper;
            _bLOCService = bLOCService;
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

            response.Message = $"Welcome back {userDto.email}";
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
            var refreshToken = new RefreshToken();
            if (_context.User.FirstOrDefault(o => o.Email == registerDto.email) != null)
            {
                response.Successful = false;
                response.Message = "User already exist";
                response.StatusCode = 400;
                return response;
            }

            _passwordHash.CreatePasswordHash(registerDto.password, out byte[] passwordHash, out byte[] passwordSalt);

            var userData = _mapper.Map<User>(registerDto);
            userData.passwordHash = passwordHash;
            userData.passwordSalt = passwordSalt;
            userData.VerificationToken = _passwordHash.CreateToken();


            refreshToken.Token = ConfirmTokens();
            refreshToken.user = userData;

            _context.User.Add(userData);
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            response.Successful = true;
            response.Message = $"User with email: {userData.Email} has been created successfully";
            response.StatusCode = 200;
            var user = _mapper.Map<UserDto>(userData);
            var token = _context.RefreshTokens.FirstOrDefault(o => o.UserId == userData.Id);

            user.RefreshToken = refreshToken.Token;
            user.JWToken = _passwordHash.CreateJWToken(userData);
            response.Data = user;

            //await _passwordHash.SendEmail(userData.Email, userData.VerificationToken);

            return response;
        }

        public async Task<bool> VerifyBvn(string bvn, string email)
        {
            var user = await _context.User.FirstOrDefaultAsync(e => e.Email == email);

            if (user == null)
                return false;
            var createCustomer = new
            {
                email = email,
                bvn = bvn,
                first_name = user.FirstName,
                last_name = user.LastName,
                phone_number = user.PhoneNumber,
                customer_type = "Personal"

            };
            /*try
            {
                var response = await "https://api.blochq.io/v1".WithHeaders(new
                {
                    authorization = "Bearer sk_live_656201fe117aa609f99dfe39656201fe117aa609f99dfe3a",
                    accept = "application/json",
                    content_type = "application/json"
                }).AppendPathSegment("/customers")
                .PostJsonAsync(createCustomer)
                .ReceiveJson<BlocResponse>();
                if(response.success== true)
                {
                    return true;
                }
                else
                {
                    return response.success;
                }
            }
            catch (Exception ex)
            {
                var message = ex;
                return false;
            }*/
            var jobId = BackgroundJob.Enqueue(()=> _bLOCService.CreateCustomers(email, bvn));

            return true;
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
        public async Task<ResponseData<UserDto>> UpdateUser(string email, UpdateUserDto userData)
        {
            var response = new ResponseData<UserDto>();
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(o => o.Email == email);
                var newEmail = await _context.User.FirstOrDefaultAsync(o => o.Email == userData.email);
                if(newEmail != null && newEmail.Email != userData.email)
                {
                    response.Message = "Email is already in use";
                    response.Successful = false;
                    response.StatusCode = 409;
                    return response;
                }
                user!.Email = userData.email;
                user.FirstName = userData.firstname;
                user.LastName = userData.lastname;
                response.StatusCode = 200;
                await _context.SaveChangesAsync();
                response.Successful = true;
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
        public async Task<ResponseData<UserDto>> UpdatePassword(string userId, UpdatePasswordDto passwordDto)
        {
            var response = new ResponseData<UserDto>();
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(o => o.Id == int.Parse(userId));
                if(!_passwordHash.VerifyPasswordHash(passwordDto.Password, user!.passwordHash, user.passwordSalt))
                {
                    response.Message = "User password is not correct";
                    response.Successful = false;
                    response.StatusCode = 400;
                    return response;
                }
                _passwordHash.CreatePasswordHash(passwordDto.newPassword, out byte[] passwordHash, out byte[] passwordSalt);
                user.passwordHash = passwordHash;
                user.passwordSalt = passwordSalt;
                await _context.SaveChangesAsync();
                response.Successful = true;
                response.StatusCode = 200;
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
 