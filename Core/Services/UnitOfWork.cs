using Core.Contracts;
using Data.Models;
using Data.Repository;
using Data.Repository.Contracts;

namespace Core.Services;

public class UnitOfWork : IUnitOfWork
{
    public IUserService Users { get; }
    public IAccountService Accounts { get; }
    public ICurrencyService Currencies { get; }

    public UnitOfWork(IRepositoryFactory repositoryFactory)
    {
        Users = new UserService(repositoryFactory.Create<User>());
        Accounts = new AccountService(repositoryFactory.Create<Account>());
        Currencies = new CurrencyService(repositoryFactory.Create<Currency>(), 
            repositoryFactory.Create<CurrencyInfo>());
    }


}