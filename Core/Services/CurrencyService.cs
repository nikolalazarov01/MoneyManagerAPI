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
    private readonly IRepository<CurrencyInfo> _currencyInfoRepository;

    public CurrencyService(IRepository<Currency> repository, IRepository<CurrencyInfo> currencyInfoRepository)
    {
        _repository = repository;
        _currencyInfoRepository = currencyInfoRepository;
    }

    public async Task<OperationResult> InsertCurrencyDetails(CurrencyInfoDto currencyInfoDto, CancellationToken token)
    {
        var operationResult = new OperationResult();
        try
        {
            var currencyResult = await GetRequiredCurrency(currencyInfoDto, token);

            if (!currencyResult.IsSuccessful) return operationResult.AppendErrors(currencyResult);

            var currencyInfo = new CurrencyInfo
            {
                CurrencyId = currencyResult.Data.Id,
                BuyRate = currencyInfoDto.BuyRate,
                SellRate = currencyInfoDto.SellRate,
                Date = DateTime.Now
            };

            operationResult = await InsertCurrencyInfoInternally(currencyInfo, token);
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
    }

    private Task<OperationResult<Currency>> GetRequiredCurrency(CurrencyInfoDto currencyInfoDto, CancellationToken token)
    {
        var filters = new List<Expression<Func<Currency, bool>>>
        {
            q => q.Code == currencyInfoDto.Currency.Code
        };
        return _repository.GetAsync(filters, null, token);
    }

    private Task<OperationResult> InsertCurrencyInfoInternally(CurrencyInfo currencyInfo, CancellationToken token)
    {
        return this._currencyInfoRepository.CreateAsync(currencyInfo, token);
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