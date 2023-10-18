using Paybliss.Models;
using Paybliss.Models.Dto;
using Paybliss.Models.HttpResp;

namespace Paybliss.Repository
{
    public interface IVtuService
    {
        Task<ResponseData<AirtimeDto>> ByAirtime(AirtimeDto airtimeDto);
        Task<ResponseData<DataPaymentReq>> DataPayment(DataPaymentReq paymentReq);
        Task<ResponseData<List<DataLookUpResponse.Plan>>> LoadNetworkData(string network);
    }
}
