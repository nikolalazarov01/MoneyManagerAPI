using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyService.Contracts
{
    public interface IApiCallService
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
