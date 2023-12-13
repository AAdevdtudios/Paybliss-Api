using Azure;
using Flurl.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paybliss.Data;
using Paybliss.Models;
using Paybliss.Models.Dto;
using Paybliss.Repository.ServicesRepo;
using System.Security.Claims;

namespace Paybliss.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IBLOCService _blocService;

        public AccountController(DataContext context, IBLOCService blocService)
        {
            _context = context;
            _blocService = blocService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseData<AccountDetails>>> GetAcount()
        {
            var email = User.FindFirst(ClaimTypes.Name)!.Value;
            var response = new ResponseData<AccountDetails>();
            try
            {
                var account = await _blocService.GetAccount(email);
                if (account == null)
                {
                    response.StatusCode = 400;
                    response.Successful = false;
                    response.Data = null;
                    response.Message = "Account not found";
                    return StatusCode(response.StatusCode, response);
                }


                response.StatusCode = 200;
                response.Successful = true;
                response.Data = account;
                response.Message = $"Account info is {account.accountNumber}";
                return StatusCode(response.StatusCode, response);

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Successful = false;
                response.StatusCode = 400;
                return StatusCode(response.StatusCode, response);
            }
        }

        [HttpGet("transactions")]
        [Authorize]
        public async Task<ActionResult<ResponseData<List<Transactions>>>> GetTransactions()
        {
            var userEmail = User.FindFirst(ClaimTypes.Name)!.Value;
            var responseData = new ResponseData<List<Transactions>>();

            responseData.Data = await _blocService.GetAcountTransactions(userEmail);
            responseData.Successful = true;
            responseData.Message = "All transactions";
            responseData.StatusCode = 200;
            return StatusCode(responseData.StatusCode, responseData);
        }

        [HttpPost("upgrade-tier1")]
        [Authorize]
        public async Task<ActionResult> UpgradeTire(UpgradeTireDto tireDto)
        {
            var email = User.FindFirst(ClaimTypes.Name)!.Value;
            var response = await _blocService.UpgradeCustomerTierOne(tireDto, email);
            return StatusCode(response.StatusCode);
        }
    }
}
