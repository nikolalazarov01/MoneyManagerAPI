using Data.Models;
using Data.Models.DTO;
using Utilities;

namespace Core.Contracts;

public interface ICurrencyService
{
    Task<OperationResult<Currency>> GetCurrencyAsync(BaseCurrencyDto currencyDto, CancellationToken token);
}