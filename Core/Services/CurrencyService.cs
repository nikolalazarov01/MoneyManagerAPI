using System.Linq.Expressions;
using Core.Contracts;
using Core.Contracts.Options;
using Data.Models;
using Data.Models.DTO;
using Data.Repository.Contracts;
using Utilities;

namespace Core.Services;

public class CurrencyService : ICurrencyService
{
    private readonly IRepository<Currency> _repository;

    public CurrencyService(IRepository<Currency> repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<Currency>> GetCurrencyAsync(IQueryOptions<Currency> queryOptions, CancellationToken token)
    {
        var operationResult = new OperationResult<Currency>();
        try
        {
            var result = await this._repository.GetAsync(queryOptions.Filters,queryOptions.Transformations, token);

            if (!result.IsSuccessful) return operationResult.AppendErrors(result);

            var currency = result.Data;
            operationResult.Data = currency;
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
    }
}