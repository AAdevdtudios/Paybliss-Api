namespace Paybliss.Models.Dto
{
    public record struct BlocReqDto(string customer_id, string preferred_bank = "Wema", string alias = "");
}
