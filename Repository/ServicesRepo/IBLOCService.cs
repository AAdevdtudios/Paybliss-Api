using Paybliss.Models;

namespace Paybliss.Repository.ServicesRepo
{
    public interface IBLOCService
    {
        Task<AccountDetails> GetAccount(string email);
        Task<bool> CreateCustomers(string email, string bvn);
        Task<List<Transactions>> GetAcountTransactions(string email);
        Task<bool> UpdateCustomer(string email);
    }
}
