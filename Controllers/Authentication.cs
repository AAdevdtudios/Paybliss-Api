// Ignore Spelling: Repo

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paybliss.Models;
using Paybliss.Models.Dto;
using Paybliss.Repository;
using System.Security.Claims;

namespace Paybliss.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class Authentication : ControllerBase
    {
        private readonly IAuthRepo _authRepo;

        public Authentication(IAuthRepo authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ResponseData<UserDto>>> GetUser([FromBody] RegisterDto registerDto)
        {
            var response = await _authRepo.RegisterUser(registerDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseData<UserDto>>> LoginUser(LoginDto loginDto)
        {
            var response = await _authRepo.LoginUser(loginDto);
            return StatusCode(response.StatusCode, response);
            
        }

        [HttpPost("verify")]
        public async Task<ActionResult<ResponseData<UserDto>>> VerifyToken(VerifyDto verifyDto)
        {
            var response = await _authRepo.VerifyUser(verifyDto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("send-token")]
        public async Task<ActionResult<ResponseData<string>>> SendToken(string email)
        {
            var response = await _authRepo.SendToken(email);
            return StatusCode(response.StatusCode,response);

        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<ResponseData<UserDto>>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var response = await _authRepo.ResetPassword(resetPasswordDto);
            return StatusCode(response.StatusCode,response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ResponseData<RefreshTokenDto>>> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var refreshToken = await _authRepo.RefreshToken(refreshTokenDto);
            return StatusCode(refreshToken.StatusCode, refreshToken);
        }

        [HttpPut("set-pin")]
        [Authorize]
        public async Task<ActionResult<ResponseData<UserDto>>> SetPin(SetPinDto setPin)
        {
            var user = User.FindFirst(ClaimTypes.Name)!.Value;
            setPin.email = user;
            var response = await _authRepo.SetPin(setPin);
            
            return StatusCode(response.StatusCode,response);
        }
        /*[HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<ResponseData<UserDto>>> GetUser()
        {
            var userEmail = User.FindFirst(ClaimTypes.Name)!.Value;
            var response = await _authRepo.GetUser(userEmail);

            return response;
        }*/
        [HttpGet("getPin")]
        [Authorize]
        public async Task<ActionResult<ResponseData<UserDto>>> GetUser([FromQuery]int pin)
        {
            var userEmail = User.FindFirst(ClaimTypes.Name)!.Value;
            var response = await _authRepo.GetUser(userEmail, pin);

            return response;
        }
    }
}
