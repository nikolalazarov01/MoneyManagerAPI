namespace Data.Models.DTO.Account;

public class AccountInfoDto
{
    public Guid Id { get; set; }

    public string Type { get; set; }
    
    public double Total { get; set; }
    
    public DateTime Date { get; set; }
}