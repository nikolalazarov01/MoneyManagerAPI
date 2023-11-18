using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Models.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Data.Models;

/// <summary>
/// Keeps data for specific user
/// </summary>
public class User : IEntity
{
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// What is the preffered currency, in which the total data will be displayed (all the foreign currency accounts'
    /// money amount will be converted to this base currency)
    /// </summary>
    [ForeignKey("BaseCurrency")]
    public Guid BaseCurrencyId { get; set; }
    public Currency BaseCurrency { get; set; }
    
    /// <summary>
    /// Collection of all the user's accounts
    /// </summary>
    public ICollection<Account>? Accounts { get; set; }
}