using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using Paybliss.Models;
using Paybliss.Models.Dto;
using Paybliss.Models.HttpResp;
using Paybliss.Repository;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using System.Diagnostics;

namespace Paybliss.Controllers
{
    [Route("api/vtu/")]
    [ApiController]
    //[Authorize]
    public class VtuServiceController : ControllerBase 
    {

        public VtuServiceController()
        {
                
        }
        [HttpPost("internet/")]
        public async Task<ActionResult<ResponseData<AirtimeDto>>> PayForAirtime([FromQuery] AirtimeDto airtimeDto)
        {
            var response = await "https://sandbox.payscribe.ng/api"
                .AppendPathSegment("/v1/data/lookup")
                .SetQueryParams(new
                {
                    network = airtimeDto.network,
                }).WithHeaders(new
                {
                    User_Agent = "Flurl",
                    Authentication = "Bearer ps_test_7b0a9d01ac341cc9b8df472a49dff4094e0bbc88dea3fab1d7fb5b65762873b6",
                    content_type = "application/json",
                })
                .WithOAuthBearerToken("ps_test_7b0a9d01ac341cc9b8df472a49dff4094e0bbc88dea3fab1d7fb5b65762873b6")
                .GetJsonAsync<InternetResponse.Details>();
            return Ok(response.amount);
        }
    }
}
