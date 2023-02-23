using Data.Models.Contracts;
using Data.Repository.Contracts;

namespace Core.Contracts;

public interface IRepositoryFactory
{
    IRepository<T> Create<T>() where T : class, IEntity;
}