using Core.Contracts;
using Core.Services;
using Core;
using Data.Repository.Contracts;
using Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyService.Contracts;
using CurrencyService.Services;

namespace CurrencyService.Extensions
{
    public static class CurrencyServicesConfigurationExtensions
    {
        public static void SetupCurrencyServices(this IServiceCollection serviceCollection)
        {
            if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddScoped<IApiCallService, ApiCallService>();
        }
    }
}
