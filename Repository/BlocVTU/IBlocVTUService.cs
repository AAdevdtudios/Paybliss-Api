using Paybliss.Models;
using Paybliss.Models.Dto;
using Paybliss.Models.HttpResp.BlocVTU;

namespace Paybliss.Repository.BlocVTU
{
    public interface IBlocVTUService
    {
        Task<ResponseData<BlocVtuServiceRes>> GetAirtime(AirtimeDto airtimeDto);
    }
}
