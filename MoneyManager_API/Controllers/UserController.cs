using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Core.Contracts;
using Data.Models;
using Data.Models.DTO;
using Data.Models.DTO.Hateoas;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyManager_API.Extensions;
using MoneyManager_API.Validation;
using Utilities;

namespace MoneyManager_API.Controllers;

[ApiController]
[Route("/user")]
public class UserController : ControllerBase
{
    private readonly IUnitOfWork _services;
    private readonly IValidator<CurrencyDto> _validatorCurrency;
    private readonly IMapper _mapper;

    public UserController(IUnitOfWork services, IValidator<CurrencyDto> validatorCurrency, IMapper mapper)
    {
        _services = services;
        _validatorCurrency = validatorCurrency;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id, CancellationToken token)
    {
        if (id == Guid.Empty) return BadRequest();
        
        var transforms = new List<Func<IQueryable<User>, IQueryable<User>>>
        {
            u => u.Include(u => u.BaseCurrency)
        };
        
        var result = await this._services.Users.GetUserById(id, transforms, token);
        if (!result.IsSuccessful) return this.Error(result);

        var representation = _mapper.Map<UserDto>(result.Data);
        representation.Links = this.GetHateoasLinks(result.Data.Id);

        return Ok(representation);
    }
    
    [HttpGet("get-current-user")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser(CancellationToken token)
    {
        var userId = await this.GetUserId();

        var transforms = new List<Func<IQueryable<User>, IQueryable<User>>>
        {
            u => u.Include(a => a.BaseCurrency),
            u => u.Include(a => a.Accounts).ThenInclude(ac => ac.Currency)
        };

        var result = await this._services.Users.GetUserById(userId, transforms, token);
        if (!result.IsSuccessful) return this.Error(result);

        var representation = _mapper.Map<UserDto>(result.Data);
        representation.Links = this.GetHateoasLinks(result.Data.Id);

        return Ok(representation);
    }
    
    [HttpPost("base-currency")]
    [Authorize]
    public async Task<IActionResult> SetBaseCurrency([FromBody] CurrencyDto currencyDto, CancellationToken cancellationToken)
    {
        var validationResult = await this.ValidateCurrencyAsync(currencyDto, cancellationToken);
        if (!validationResult.IsValid) return this.ValidationError(validationResult);

        var userId = await this.GetUserId();

        var result = await this.SetBaseCurrencyInternally(currencyDto, userId, cancellationToken);
        if (!result.IsSuccessful) return this.Error(result);

        return CreatedAtAction("GetCurrentUser", null, result.Data);
    }
    
    private async Task<ValidationResult> ValidateCurrencyAsync(CurrencyDto currencyDto, CancellationToken token)
    {
        if (this._validatorCurrency is null) return null;
        return await this._validatorCurrency.ValidateAsync(currencyDto, token);
    }

    private async Task<OperationResult<UserDto>> SetBaseCurrencyInternally(CurrencyDto currencyDto,
        Guid userId, CancellationToken token)
    {
        var operationResult = new OperationResult<UserDto>();
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

            var representation = _mapper.Map<UserDto>(setCurrencyResult.Data);
            representation.Links = this.GetHateoasLinks(setCurrencyResult.Data.Id);
            operationResult.Data = representation;
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
    }

    private IEnumerable<HateoasLink> GetHateoasLinks(Guid id)
    {
        var links = new List<HateoasLink>()
        {
            new()
            {
                Url = this.AbsoluteUrl("GetCurrentUser", "User", null), Method = HttpMethods.Get,
                Rel = "self"
            },
            new()
            {
                Url = this.AbsoluteUrl("SetBaseCurrency", "User", null), Method = HttpMethods.Post,
                Rel = "create"
            },
            new()
            {
                Url = this.AbsoluteUrl("GetUserById", "User", new { Id = id }), Method = HttpMethods.Get,
                Rel = "self"
            }
        };

        return links;
    }
}