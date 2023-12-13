using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Paybliss.Data;
using Paybliss.Models;
using Paybliss.Models.Dto;
using Paybliss.Models.HttpResp;
using Reloadly.Airtime.Dto.Response;

namespace Paybliss.Repository.ServicesRepo
{
    public class BLOCServiice : IBLOCService
    {
        private readonly DataContext _context;

        public BLOCServiice(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateCustomers(string email, string bvn)
        {
            var user = _context.User.FirstOrDefault(x => x.Email == email);
            string apiUrl = Environment.GetEnvironmentVariable("BLOCURL") ?? "";
            string apiKey = Environment.GetEnvironmentVariable("BLOCKEY") ?? "";

            if (user == null)
            {
                return await Task.FromResult(false);
            }
            CreateAccount createCustomer = new CreateAccount
            {
                email = email,
                bvn = bvn,
                first_name = user.FirstName,
                last_name = user.LastName,
                phone_number = user.PhoneNumber,
                customer_type = "Personal"

            };
            var response = await "https://api.blochq.io/v1".WithHeaders(new
            {
                authorization = "Bearer sk_live_656201fe117aa609f99dfe39656201fe117aa609f99dfe3a",
                accept = "application/json",
                content_type = "application/json"
            }).AppendPathSegment("/customers").AllowAnyHttpStatus()
            .PostJsonAsync(createCustomer)
            .ReceiveJson<BlocResponse>();

            if (response.success == false)
                return await Task.FromResult(false);

            user.custormerId = response.data.id;
            user.tier = Tier.Tier0;
            user.bvn = bvn;
            BlocReqDto blocReq = new BlocReqDto
            {
                customer_id = response.data.id,
                preferred_bank = "Wema",
                alias = "blis"

            };
            var accountRes = await "https://api.blochq.io/v1".WithHeaders(new
            {
                authorization = "Bearer sk_live_656201fe117aa609f99dfe39656201fe117aa609f99dfe3a",
                accept = "application/json",
                content_type = "application/json"
            })
                .AppendPathSegment("/accounts").AllowAnyHttpStatus()
                .PostJsonAsync(blocReq)
                .ReceiveJson<BlocAccountRes>();
            if (accountRes.success == false)
                return await Task.FromResult(false);
            AccountDetails account = new AccountDetails() { 
                accountName = accountRes.data.name,
                accountNumber = accountRes.data.account_number,
                bank = "Wema",
                owner = user,
                amount = "0",
                bank_name = accountRes.data.bank_name,
                reference = accountRes.data.id,
                currency = "NGN",
                userId = user.Id,
                accountId = accountRes.data.id,
            };
            user.Account = account;

            await _context.SaveChangesAsync();
            return await Task.FromResult(true);
        }
        
        public async Task<bool> UpdateCustomer(string email)
        {
            var user = await _context.User.FirstOrDefaultAsync(i => i.Email == email);
            if (user == null) return false;
            var response = await "https://api.blochq.io/v1".WithHeaders(new
            {
                authorization = "Bearer sk_live_656201fe117aa609f99dfe39656201fe117aa609f99dfe3a",
                accept = "application/json",
                content_type = "application/json"
            }).AllowAnyHttpStatus()
            .AppendPathSegment($"/customers/{user.custormerId}")
            .PutJsonAsync(new
            {
                email = user.Email,
                phone_number = user.PhoneNumber,
                bvn = user.bvn,
                customer_type = "individual"
            });
            if (response.StatusCode != 200)
                return false;
            return true;
        }

        public async Task<AccountDetails> GetAccount(string email)
        {
            var user = await _context.User.Include(c => c.Account).FirstOrDefaultAsync(i => i.Email == email);
             
            var accountDetails = await "https://api.blochq.io/v1".WithHeaders(new
            {
                authorization = "Bearer sk_live_656201fe117aa609f99dfe39656201fe117aa609f99dfe3a",
                accept = "application/json",
                content_type = "application/json"
            }).AppendPathSegment($"/accounts/number/{user!.Account!.accountNumber}")
            .AllowAnyHttpStatus()
            .GetJsonAsync<BlocAccount>();

            user.Account.amount = accountDetails.data.balance.ToString();
            user.Account.accountId = accountDetails.data.id.ToString();
            await _context.SaveChangesAsync();
            return user.Account;
        }

        public async Task<List<Transactions>> GetAcountTransactions(string email)
        {
            var user = await _context.User.Include(t=>t.transactions).FirstOrDefaultAsync(i=> i.Email == email);

            return user!.transactions.ToList();
        }

        public async Task<ResponseData<bool>> UpgradeCustomerTierOne(UpgradeTireDto tireDto, string email)
        {
            var user = await _context.User.FirstOrDefaultAsync(i => i.Email == email);
            var responseData = new ResponseData<bool>();
            if (user == null)
            {
                responseData.Successful = false;
                responseData.StatusCode = 400;
                return responseData;
            }
            var response = await "https://api.blochq.io/v1".WithHeaders(new
            {
                authorization = "Bearer sk_live_656201fe117aa609f99dfe39656201fe117aa609f99dfe3a",
                accept = "application/json",
                content_type = "application/json"
            }).AllowAnyHttpStatus()
            .AppendPathSegment($"/customers/upgrade/t1/{user.custormerId}")
            .PutJsonAsync(new
            {
                place_of_birth = tireDto.place_of_birth,
                dob = tireDto.dob,
                gender = tireDto.gender,
                country = tireDto.country,
                address= new
                {
                    street = tireDto.street,
                    city = tireDto.city,
                    state = tireDto.state,
                    country = tireDto.country,
                    postal_code = tireDto.postal_code,
                },
                image = tireDto.image,
            });

            if(response.StatusCode != 200)
            {
                responseData.StatusCode = 400;
                responseData.Message = "Cant do anything";
                responseData.Successful = false;
                return responseData;
            }

            user.tier = Tier.Tier1;
            await _context.SaveChangesAsync();
            responseData.Successful = true;
            responseData.StatusCode = 200;
            responseData.Data = true;
            return responseData;
        }
    }
}
