using Data.Models.DTO.Base;

namespace Data.Models.DTO.Account;

public class NewAccountCreatedDto : BaseDtoModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CurrencyDto Currency { get; set; }
    public Guid UserId { get; set; }
    public double Total { get; set; }
}