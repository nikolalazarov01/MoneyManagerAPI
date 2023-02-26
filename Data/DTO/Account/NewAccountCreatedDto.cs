namespace Data.Models.DTO.Account;

public class NewAccountCreatedDto
{
    public CurrencyDto Currency { get; set; }
    public Guid UserId { get; set; }
    public double Total { get; set; }
}