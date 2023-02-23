using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Data.Models;

public class User 
{
    [Key]
    public Guid id { get; set; }
    
    [ForeignKey("BaseCurrency")]
    public Guid BaseCurrencyId { get; set; }
    public Currency BaseCurrency { get; set; }
    
    public ICollection<Account>? Accounts { get; set; }
}