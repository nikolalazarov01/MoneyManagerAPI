using Core.Contracts;
using Data.Models;
using Data.Repository.Contracts;

namespace Core.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _repository;

    public UserService(IRepository<User> repository)
    {
        _repository = repository;
    }
}