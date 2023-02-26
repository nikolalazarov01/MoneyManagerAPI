using Data.Models.DTO.Base;

namespace Data.Models.DTO.Account;

public class AccountDto : BaseDtoModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CurrencyDto Currency { get; set; }
    public double Total { get; set; }
}