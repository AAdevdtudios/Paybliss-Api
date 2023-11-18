using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult> PayForData([FromBody] DataPaymentReq paymentReq)
        {
            var response = await vtuService.DataPayment(paymentReq);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("cable")]
        public async Task<ActionResult> SaveCable([FromBody] CablesDto cablesDto)
        {
            var response = await vtuService.CreateCableNetwork(cablesDto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
