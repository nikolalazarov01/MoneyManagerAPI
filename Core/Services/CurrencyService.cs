using System.Linq.Expressions;
using Core.Contracts;
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

    public async Task<OperationResult<Currency>> GetCurrencyAsync(BaseCurrencyDto currencyDto, CancellationToken token)
    {
        var operationResult = new OperationResult<Currency>();
        try
        {
            var result = await this._repository.GetAsync(new List<Expression<Func<Currency, bool>>>
            {
                c =>
                    c.Code.ToLower() == currencyDto.Code.ToLower()
            }, token);

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