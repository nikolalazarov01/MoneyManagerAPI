namespace Data.Models.DTO.Account;

public class UserAccountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CurrencyDto Currency { get; set; }
    public double Total { get; set; }
}