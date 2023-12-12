using Flurl.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Identity.Client;
using Paybliss.Models;
using Paybliss.Models.Dto;
using Paybliss.Models.HttpResp.BlocVTU;

namespace Paybliss.Repository.BlocVTU
{
    public class BlocVTUService : IBlocVTUService
    {
        public async Task<ResponseData<BlocVtuServiceRes>> GetAirtime(AirtimeDto airtimeDto)
        {
            var response = new ResponseData<BlocVtuServiceRes>();
            try
			{
                var billRes = await "https://api.blochq.io/v1".WithHeaders(new
                {
                    authorization = "Bearer sk_live_656201fe117aa609f99dfe39656201fe117aa609f99dfe3a",
                    accept = "application/json",
                    content_type = "application/json"
                }).AppendPathSegment("/bills/operators")
                .SetQueryParam("bill", "telco")
                .GetJsonAsync<BlocVtuServiceRes>();

                string productId = billRes.data.FirstOrDefault(i => i.name == $"{char.ToUpper(airtimeDto.network[0]) + airtimeDto.network.Substring(1)}")!.id;
                var productRes = await "https://api.blochq.io/v1".WithHeaders(new
                {
                    authorization = "Bearer sk_live_656201fe117aa609f99dfe39656201fe117aa609f99dfe3a",
                    accept = "application/json",
                    content_type = "application/json"
                }).AppendPathSegment($"/bills/operators/{productId}/products")
                .SetQueryParam("bill", "telco").GetJsonAsync<OperatorProductRes>();

                string operator_id = productRes.data.FirstOrDefault(i => i.category == "pctg_xkf8nz3rFLjbooWzppWBG6")!.id;


                var purchase = await "https://api.blochq.io/v1".WithHeaders(new
                {
                    authorization = "Bearer sk_live_656201fe117aa609f99dfe39656201fe117aa609f99dfe3a",
                    accept = "application/json",
                    content_type = "application/json"
                }).AppendPathSegment("/bills/payment")
                .AllowAnyHttpStatus()
                .SetQueryParam("bill", "telco")
                .PostJsonAsync(new
                {
                    amount = airtimeDto.amount,
                    product_id = productId,
                    operator_id = operator_id,
                    account_id = airtimeDto.account_id,
                    device_details = new
                    {
                        beneficiary_msisdn = airtimeDto.phoneNumber,
                    }
                });

                if(purchase.StatusCode == 400)
                {
                    var result = await purchase.GetJsonAsync<BlocError>();
                    response.StatusCode = 400;
                    response.Data = null;
                    response.Successful = result.Success;
                    response.Message = result.Message;
                    return response;
                }

                response.StatusCode = 200;
                response.Data = billRes;
                response.Message = "Sucess";
                return response;
            }
			catch (FlurlHttpException ex)
			{
                response.StatusCode = 400;
                response.Data = null;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
