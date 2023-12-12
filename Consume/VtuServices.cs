using AutoMapper;
using Flurl;
using Flurl.Http;
using Paybliss.Data;
using Paybliss.Models;
using Paybliss.Models.Dto;
using Paybliss.Models.HttpResp;
using Paybliss.Repository;

namespace Paybliss.Consume
{
    public class VtuServices : IVtuService
    {
        private static string url;
        private static string test_key;
        private static string live_key;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public VtuServices(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            url = Environment.GetEnvironmentVariable("ENDPOINTS");
            test_key = Environment.GetEnvironmentVariable("PAYSCRIBE_KEY_TEST");
            live_key = Environment.GetEnvironmentVariable("PAYSCRIBE_KEY_LIVE");
        }

        public async Task<ResponseData<AirtimeDto>> BuyAirtime(AirtimeDto airtimeDto)
        {
            var response = new ResponseData<AirtimeDto>();
			try
			{
                var request = await url.AppendPathSegment("/v1/airtime").AllowAnyHttpStatus()
                    .WithHeaders(new
                    {
                        User_Agent = "Flurl",
                        content_type = "application/json",
                        Authorization = $"Bearer {live_key}"
                    })
                    .PostJsonAsync(airtimeDto);
                response.StatusCode = request.StatusCode;
                response.Successful = request.StatusCode == 200 ? true : false;
                response.Message = HandleExceptios(response.StatusCode);
                response.Data = airtimeDto;

                return response;
			}
            catch (Exception e)
			{
                response.StatusCode = 500;
                response.Message += e.Message;
                response.Data = null;
                return response;
			}
        }

        public async Task<ResponseData<List<DataLookUpResponse.Plan>>> LoadNetworkData(string network)
        {
            var response = new ResponseData<List<DataLookUpResponse.Plan>>();
            try
            {
                DataLookUpResponse.ApiResponse request = await url.AppendPathSegment("/v1/data/lookup").AllowAnyHttpStatus()
                    .WithHeaders(new
                    {
                        User_Agent = "Flurl",
                        content_type = "application/json",
                        Authorization = test_key
                    })
                    .SetQueryParam("network",network)
                    .GetJsonAsync<DataLookUpResponse.ApiResponse>();
                response.StatusCode = request.Status != false ? 200 : 406;
                response.Successful = request.Status != false ? true : false;
                response.Message = HandleExceptios(response.StatusCode);
                response.Data = request.Status != false? request.Message!.Details[0].Plans: null;
                return response;
            }
            catch (Exception ex)
            {

                response.StatusCode = 406;
                response.Message = HandleExceptios(406);
                response.Successful = false;
                response.Data = null;
                return response;
            }
        }
        
        public async Task<ResponseData<DataPaymentReq>> DataPayment(DataPaymentReq paymentReq)
        {
            var response = new ResponseData<DataPaymentReq>();
            try
            {
                var request = await url.AppendPathSegment("/v1/data/vend").AllowAnyHttpStatus()
                    .WithHeaders(new
                    {
                        User_Agent = "Flurl",
                        content_type = "application/json",
                        Authorization = live_key
                    })
                    .PostJsonAsync(paymentReq);
                response.StatusCode = request.StatusCode;
                response.Successful = request.StatusCode == 200?true : false;
                response.Message = HandleExceptios(response.StatusCode);
                response.Data = paymentReq;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message += ex.Message;
                response.Data = null;
                return response;
            }
        }

        public async Task<ResponseData<CablesDto>> CreateCableNetwork(CablesDto cables)
        {
            var response = new ResponseData<CablesDto>();
            try
            {
                var cable = _mapper.Map<CableValues>(cables);
                _context.Cables.Add(cable);
                await _context.SaveChangesAsync();
                response.StatusCode = 200;
                response.Data = cables;
                response.Message = "Successful";
                response.Successful = true;
                return response;
            }
            catch (Exception e)
            {
                response.StatusCode = 400;
                response.Message = "Failed server error";
                response.Successful = false;
                return response;
            }
        }

        #region This section handles Errors and messages
        private String HandleExceptios(int code)
        {
            String message = "";
            switch (code)
            {
                case 200:
                    message = "Success";
                    break;
                case 201:
                    message = "Transaction Pending";
                    break;
                case 400:
                    message = "Something missing in your body request";
                    break;
                case 404:
                    message = "Page not found";
                    break;
                case 405:
                    message = "Duplicate Transaction";
                    break;
                case 406:
                    message = "Missing Required Information, Please check that you have provided all mandatory information\r\n";
                    break;
                case 410:
                    message = "Insufficient money in your wallet";
                    break;
                default:
                    message = "Some server-side error";
                    break;
            }
            return message;
        }
        #endregion
    }
}
