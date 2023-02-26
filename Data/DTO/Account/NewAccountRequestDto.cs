namespace Data.Models.DTO.Account;

public class NewAccountRequestDto
{
    public string Name { get; set; }
    public CurrencyDto Currency { get; set; }
}