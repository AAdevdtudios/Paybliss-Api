﻿using Paybliss.Models;
using Paybliss.Models.Dto;
using Paybliss.Models.HttpResp;

namespace Paybliss.Repository
{
    public interface IVtuService
    {
        Task<ResponseData<AirtimeDto>> BuyAirtime(AirtimeDto airtimeDto);
        Task<ResponseData<CablesDto>> CreateCableNetwork(CablesDto cables);
        Task<ResponseData<DataPaymentReq>> DataPayment(DataPaymentReq paymentReq);
        Task<ResponseData<List<DataLookUpResponse.Plan>>> LoadNetworkData(string network);
    }
}
