using Tiny.RestClient;

namespace Paybliss.Models.Dto
{
    public record struct CreateAccount(string first_name, string last_name, string bvn, string email, string phone_number, string customer_type);
}
