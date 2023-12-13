using Paybliss.Models;
using Paybliss.Models.Dto;

namespace Paybliss.Repository.ServicesRepo
{
    public interface IBLOCService
    {
        Task<AccountDetails> GetAccount(string email);
        Task<bool> CreateCustomers(string email, string bvn);
        Task<List<Transactions>> GetAcountTransactions(string email);
        Task<bool> UpdateCustomer(string email);
        Task<ResponseData<bool>> UpgradeCustomerTierOne(UpgradeTireDto tireDto, string email);
    }
}
