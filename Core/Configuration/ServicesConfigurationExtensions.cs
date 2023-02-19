using System.Runtime.CompilerServices;
using Core.Services;
using Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Configuration;

public static class ServicesConfigurationExtensions
{
    public static void SetupServices(this IServiceCollection serviceCollection)
    {
        if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));

        serviceCollection.AddScoped(typeof(UserRepository));
        serviceCollection.AddScoped(typeof(UserService));
    }
}