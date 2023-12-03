// Ignore Spelling: Repo

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Paybliss.Models;
using Paybliss.Models.Dto;
using Paybliss.Repository;
using Paybliss.Repository.ServicesRepo;
using System.Security.Claims;

namespace Paybliss.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class Authentication : ControllerBase
    {
        private readonly IAuthRepo _authRepo;
        private readonly BLOCServiice _blocService;

        public Authentication(IAuthRepo authRepo, BLOCServiice blocService)
        {
            _authRepo = authRepo;
            _blocService = blocService;
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

        [HttpPost("bvn")]
        public async Task<ActionResult<ResponseData<bool>>> VeriftBvn(ValidateBvnDto validate)
        {
            var response = new ResponseData<bool>();
            bool res = await _authRepo.VerifyBvn(validate.bvn, validate.email);
            response.Data = res;

            if (res == false)
                response.StatusCode = 400;
            else
                response.StatusCode = 200;

            response.Successful = res;

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
        [HttpPost("reset-pass")]
        [Authorize]
        public async Task<ActionResult<ResponseData<UserDto>>> ChangePassword(UpdatePasswordDto resetPasswordDto)
        {
            var userEmail = User.FindFirst(ClaimTypes.Hash)!.Value;
            var response = await _authRepo.UpdatePassword(userEmail, resetPasswordDto);
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
        // /getpin&pin=1232
        [HttpGet("getPin")]
        [Authorize]
        public async Task<ActionResult<ResponseData<UserDto>>> GetUser([FromQuery]int pin)
        {
            var userEmail = User.FindFirst(ClaimTypes.Name)!.Value;
            var response = await _authRepo.GetUser(userEmail, pin);

            return StatusCode(response.StatusCode, response);
        }
        [HttpPost("update-user")]
        [Authorize]
        public async Task<ActionResult<ResponseData<UserDto>>> UpdateUser([FromBody]UpdateUserDto user)
        {
            var userEmail = User.FindFirst(ClaimTypes.Name)!.Value;
            var response = await _authRepo.UpdateUser(userEmail,user);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("Account")]
        [Authorize]
        public async Task<ActionResult<ResponseData<AccountDetails>>> GetAcount()
        {
            var userEmail = User.FindFirst(ClaimTypes.Name)!.Value;
            var res = await _blocService.GetAccountDetails(userEmail);
            return StatusCode(res.StatusCode, res);
        }
    }
}
