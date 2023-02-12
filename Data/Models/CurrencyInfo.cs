using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models;

public class CurrencyInfo
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("Currency")]
    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; }
    
    public double BuyRate { get; set; }
    
    public double SellRate { get; set; }
}