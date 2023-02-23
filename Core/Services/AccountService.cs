using Core.Contracts;
using Data.Models;
using Data.Repository.Contracts;

namespace Core.Services;

public class AccountService : IAccountService
{
    private readonly IRepository<Account> _repository;

    public AccountService(IRepository<Account> repository)
    {
        _repository = repository;
    }
}