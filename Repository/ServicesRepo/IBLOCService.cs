namespace Paybliss.Repository.ServicesRepo
{
    public interface IBLOCService
    {
        Task<bool> CreateCustomers(string email, string bvn);
    }
}
