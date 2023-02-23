using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Core.Contracts;
using Data.Models;
using Data.Models.DTO;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager_API.Extensions;
using MoneyManager_API.Validation;
using Utilities;

namespace MoneyManager_API.Controllers;

[ApiController]
[Route("/user")]
public class UserController : ControllerBase
{
    private readonly IUnitOfWork _services;
    private readonly IValidator<BaseCurrencyDto> _validatorCurrency;
    private readonly IMapper _mapper;

    public UserController(IUnitOfWork services, IValidator<BaseCurrencyDto> validatorCurrency, IMapper mapper)
    {
        _services = services;
        _validatorCurrency = validatorCurrency;
        _mapper = mapper;
    }

    [HttpGet("get-info")]
    [Authorize]
    public async Task<IActionResult> GetUserById(CancellationToken token)
    {
        var userId = await this.GetUserId();

        var result = await this._services.Users.GetUserById(userId, token);
        if (!result.IsSuccessful) return this.Error(result);

        var representation = _mapper.Map<UserDto>(result.Data);

        return Ok(representation);
    }
    
    [HttpPost("base-currency")]
    [Authorize]
    public async Task<IActionResult> SetBaseCurrency([FromBody] BaseCurrencyDto currencyDto, CancellationToken cancellationToken)
    {
        var validationResult = await this.ValidateCurrencyAsync(currencyDto, cancellationToken);
        if (!validationResult.IsValid) return this.ValidationError(validationResult);

        var userId = await this.GetUserId();

        var result = await this.SetBaseCurrencyInternally(currencyDto, userId, cancellationToken);
        if (!result.IsSuccessful) return this.Error(result);

        return Ok();
    }
    
    private async Task<ValidationResult> ValidateCurrencyAsync(BaseCurrencyDto currencyDto, CancellationToken token)
    {
        if (this._validatorCurrency is null) return null;
        return await this._validatorCurrency.ValidateAsync(currencyDto, token);
    }

    private async Task<OperationResult<BaseCurrencyDto>> SetBaseCurrencyInternally(BaseCurrencyDto currencyDto,
        Guid userId, CancellationToken token)
    {
        var operationResult = new OperationResult<BaseCurrencyDto>();
        try
        {
            var result = await this._services.Currencies.GetCurrencyAsync(currencyDto, token);
            if (!result.IsSuccessful) return operationResult.AppendErrors(result);

            Currency currency;
            
            if (result.Data is not null)
            {
                currency = result.Data;
            }
            else
            {
                currency = new Currency
                {
                    Code = currencyDto.Code
                };
            }
            
            var setCurrencyResult = await this._services.Users.SetUserBaseCurrency(currency, userId, token);

            if (!setCurrencyResult.IsSuccessful) return operationResult.AppendErrors(setCurrencyResult);

            var representation = _mapper.Map<BaseCurrencyDto>(setCurrencyResult.Data);
            operationResult.Data = representation;
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
    }
    
    private async Task<Guid> GetUserId()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var userId = jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value;
        
        return Guid.Parse(userId);
    }
}