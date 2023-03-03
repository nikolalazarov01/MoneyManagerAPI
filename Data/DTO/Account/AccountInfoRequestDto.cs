namespace Data.Models.DTO.Account;

public class AccountInfoRequestDto
{
    public Guid AccountId { get; set; }

    public string Type { get; set; }
    
    public double Total { get; set; }
}