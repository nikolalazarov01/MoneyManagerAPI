using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Models.Contracts;

namespace Data.Models;

/// <summary>
/// Keeps records of a specific currency's data, such as buy and sell rates for specific day
/// </summary>
public class CurrencyInfo : IEntity
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("Currency")]
    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; }
    
    public double BuyRate { get; set; }
    
    public double SellRate { get; set; }
    
    public DateTime Date { get; set; }
}