using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Models.Contracts;
using Data.Utils;

namespace Data.Models;

public class AccountInfo : IEntity
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("Account")]
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
    
    public AccountInfoType Type { get; set; }
    
    public double Total { get; set; }
    
    public DateTime Date { get; set; }
}