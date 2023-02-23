using Core.Contracts;
using Data.Models.Contracts;
using Data.Repository;
using Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Core;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly DbContext _db;

    public RepositoryFactory(DbContext db)
    {
        _db = db;
    }

    public IRepository<T> Create<T>() where T : class, IEntity
    {
        return new Repository<T>(_db);
    }
}