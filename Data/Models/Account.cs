using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public class Account
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("Currency")]
    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; }
    
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public ICollection<AccountInfo>? AccountInfos { get; set; }

    public double Total { get; set; }

    public Account()
    {
        this.Total = 0;
        AccountInfos = new List<AccountInfo>();
    }
}