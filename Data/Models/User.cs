using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Data.Models;

public class User : IdentityUser
{
    [ForeignKey("BaseCurrency")]
    public Guid BaseCurrencyId { get; set; }
    public Currency BaseCurrency { get; set; }
    
    [ForeignKey("Account")]
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
}