using Paybliss.Models;

namespace Paybliss.Repository.ServicesRepo
{
    public interface IBLOCService
    {
        Task<AccountDetails> GetAccount(string email);
        Task<bool> CreateCustomers(string email, string bvn);
    }
}
