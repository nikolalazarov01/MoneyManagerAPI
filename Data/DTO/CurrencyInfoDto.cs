namespace Data.Models.DTO;

public class CurrencyInfoDto
{
    public CurrencyDto Currency { get; set; }
    
    public double BuyRate { get; set; }
    
    public double SellRate { get; set; }
}