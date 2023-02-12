using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class Currency
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Code { get; set; }
    
    public ICollection<CurrencyInfo>? CurrencyInfos { get; set; }
}