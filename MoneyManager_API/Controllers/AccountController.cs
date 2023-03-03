using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AutoMapper;
using Core.Contracts;
using Data.Models;
using Data.Models.DTO.Account;
using Data.Models.DTO.Hateoas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyManager_API.Extensions;
using Utilities;

namespace MoneyManager_API.Controllers;

[ApiController]
[Route("/accounts")]
public class AccountController : ControllerBase
{
    private readonly IUnitOfWork _services;
    private IMapper _mapper;

    public AccountController(IUnitOfWork services, IMapper mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetAccountById([FromRoute] Guid id, CancellationToken token)
    {
        var userId = await this.GetUserId();

        var filters = new List<Expression<Func<Account, bool>>>
        {
            a => a.Id == id
        };
        var transformations = new List<Func<IQueryable<Account>, IQueryable<Account>>>
        {
            a => a.Include(ac => ac.Currency),
            a => a.Include(ac => ac.AccountInfos)
        };

        var result = await this._services.Accounts.GetAccount(filters, transformations, token);
        if (!result.IsSuccessful) return this.Error(result);

        if (result.Data.UserId != userId)
            return BadRequest("No access to different user's accounts!");
        
        var representation = this._mapper.Map<AccountDto>(result.Data);
        representation.Links = this.GetHateoasLinks(result.Data.Id);
        
        return Ok(result.Data);
    }

    [HttpGet("user-accounts")]
    [Authorize]
    public async Task<IActionResult> GetUserAccounts(CancellationToken token)
    {
        var userId = await this.GetUserId();
        if (userId == Guid.Empty)
            return BadRequest();

        var result = await this._services.Accounts.GetUserAccounts(userId, token);
        if (!result.IsSuccessful) return this.Error(result);

        var representation = this._mapper.Map<IEnumerable<AccountDto>>(result.Data);
        foreach (var accountDto in representation)
        {
            accountDto.Links = this.GetHateoasLinks(accountDto.Id);
        }
        
        return Ok(representation);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> RemoveById([FromRoute] Guid id, CancellationToken token)
    {
        var userId = await this.GetUserId();
        if (userId == Guid.Empty)
            return BadRequest();

        var result = await this.DeleteAccount(id, token);
        if (!result.IsSuccessful) return this.Error(result);

        return NoContent();
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddUserAccount([FromBody] NewAccountRequestDto accountRequestDto, CancellationToken token)
    {
        var userId = await this.GetUserId();
        if (userId == Guid.Empty)
            return BadRequest();

        var result = await this.CreateAccountAsync(accountRequestDto, userId, token);
        if (!result.IsSuccessful) return this.Error(result);
        
        return CreatedAtAction("GetAccountById", new {Id = result.Data.Id}, result.Data);
    }

    private async Task<OperationResult<NewAccountCreatedDto>> CreateAccountAsync(NewAccountRequestDto accountRequestDto, Guid userId, CancellationToken token)
    {
        var operationResult = new OperationResult<NewAccountCreatedDto>();
        try
        {
            var transforms = new List<Func<IQueryable<User>, IQueryable<User>>>
            {
                u => u.Include(a => a.Accounts)
            };
            var user = await this._services.Users.GetUserById(userId, transforms, token);
            if (!user.IsSuccessful) return operationResult.AppendErrors(user);

            var account = this._mapper.Map<Account>(accountRequestDto);
            var result = await this._services.Accounts.AddNewAccountAsync(user.Data, account, token);

            if (!result.IsSuccessful) return operationResult.AppendErrors(result);

            var representation = this._mapper.Map<NewAccountCreatedDto>(result.Data);
            representation.Links = this.GetHateoasLinks(representation.Id);
            operationResult.Data = representation;
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
    }

    private async Task<OperationResult> DeleteAccount(Guid accountId, CancellationToken token)
    {
        var operationResult = new OperationResult();

        try
        {
            var result = await this._services.Accounts.DeleteAccountAsync(accountId, token);
            if (!result.IsSuccessful) return operationResult.AppendErrors(result);
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
    }

    private IEnumerable<HateoasLink> GetHateoasLinks(Guid accountId)
    {
        var links = new List<HateoasLink>()
        {
            new()
            {
                Url = this.AbsoluteUrl("GetAccountById", "Account", new {Id = accountId}), Method = HttpMethods.Get,
                Rel = "self"
            },
            new()
            {
                Url = this.AbsoluteUrl("RemoveById", "Account", new {Id = accountId}), Method = HttpMethods.Delete,
                Rel = "delete"
            }
        };

        return links;
    }
}