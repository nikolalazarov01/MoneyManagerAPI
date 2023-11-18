using System.ComponentModel.DataAnnotations;
using Data.Models.Contracts;

namespace Data.Models;

/// <summary>
/// Keeps the currency's code ("BGN", "EUR"...) and a collection of it's infos (buy/sell rates)
/// </summary>
public class Currency : IEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Code { get; set; }
    
    public ICollection<CurrencyInfo>? CurrencyInfos { get; set; }

    public Currency()
    {
    }

    public Currency(string code)
    {
        Code = code;
    }
}