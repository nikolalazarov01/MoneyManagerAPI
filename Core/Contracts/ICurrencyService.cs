using Core.Contracts.Options;
using Data.Models;
using Data.Models.DTO;
using Utilities;

namespace Core.Contracts;

public interface ICurrencyService
{
    Task<OperationResult<Currency>> GetCurrencyAsync(IQueryOptions<Currency> queryOptions, CancellationToken token);
}