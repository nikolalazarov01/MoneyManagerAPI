
using Data.Repository.Contracts;

namespace Core.Contracts;

public interface IUnitOfWork
{
    IUserService Users { get; }
    IAccountService Accounts { get; }
    ICurrencyService Currencies { get; }
}