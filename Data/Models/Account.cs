using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Models.Contracts;

namespace Data.Models;

/// <summary>
/// Keeps data about a user's account (a user can have multiple accounts in different currencies)
/// </summary>
public class Account : IEntity
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    [ForeignKey("Currency")]
    public Guid CurrencyId { get; set; }
    public Currency Currency { get; set; }
    
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public ICollection<TransactionInfo>? TransactionInfos { get; set; }

    /// <summary>
    /// Data of the total amount of money in the current account in the account's currency
    /// </summary>
    public double Total { get; set; }

    public Account()
    {
        this.Total = 0;
        TransactionInfos = new List<TransactionInfo>();
    }
}