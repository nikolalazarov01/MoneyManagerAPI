using System.Runtime.CompilerServices;
using Core.Contracts;
using Core.Services;
using Data.Repository;
using Data.Repository.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Configuration;

public static class ServicesConfigurationExtensions
{
    public static void SetupServices(this IServiceCollection serviceCollection)
    {
        if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));

        serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        serviceCollection.AddScoped<IRepositoryFactory, RepositoryFactory>();
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IAccountService, AccountService>();
        serviceCollection.AddScoped<ICurrencyService, CurrencyService>();
    }
}