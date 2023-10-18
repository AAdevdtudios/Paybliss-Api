using Azure;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Paybliss.Models;
using Paybliss.Models.Dto;
using Paybliss.Models.HttpResp;
using Paybliss.Repository;

namespace Paybliss.Controllers
{
    [Route("api/vtu/")]
    [ApiController]
    //[Authorize]
    public class VtuServiceController : ControllerBase 
    {
        private readonly IVtuService vtuService;
        public VtuServiceController(IVtuService _vtuService)
        {
            vtuService = _vtuService;
        }
        [HttpPost("airtime/")]
        public async Task<ActionResult<ResponseData<AirtimeDto>>> PayForAirtime([FromQuery] AirtimeDto airtimeDto)
        {
            var response = await vtuService.ByAirtime(airtimeDto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("data")]
        public async Task<ActionResult<ResponseData<List<DataLookUpResponse.Plan>>>> GetListOfData([FromQuery]string network)
        {
            var response = await vtuService.LoadNetworkData(network);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("data")]
        public async Task<ActionResult<ResponseData<DataPaymentReq>>> PayForData([FromBody] DataPaymentReq paymentReq)
        {
            var response = await vtuService.DataPayment(paymentReq);
            return StatusCode(response.StatusCode, response);
        }
    }
}
