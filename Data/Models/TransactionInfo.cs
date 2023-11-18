using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Models.Contracts;
using Data.Utils;

namespace Data.Models;

/// <summary>
/// Keeps records of specific transaction for an account (inserting money, etc.)
/// </summary>
public class TransactionInfo : IEntity
{
    [Key]
    public Guid Id { get; set; }
    
    [ForeignKey("Account")]
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
    
    /// <summary>
    /// Type of the transaction - income, outcome...
    /// </summary>
    public TransactionInfoType Type { get; set; }
    
    /// <summary>
    /// Total money that went into the transaction
    /// </summary>
    public double Total { get; set; }
    
    public DateTime Date { get; set; }
}