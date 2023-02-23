using System.ComponentModel.DataAnnotations;
using Data.Models.Contracts;

namespace Data.Models;

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