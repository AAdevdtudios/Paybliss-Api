using Paybliss.Models;

namespace Paybliss.Repository
{
    public interface IBlocService
    {
        Task<ResponseData<AccountDetails>> GetAccountDetails(string email);
    }
}
