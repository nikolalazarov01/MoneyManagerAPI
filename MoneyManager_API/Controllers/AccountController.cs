using AutoMapper;
using Core.Contracts;
using Data.Models;
using Data.Models.DTO.Account;
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

    [HttpPost("add")]
    [Authorize]
    public async Task<IActionResult> AddUserAccount([FromBody] NewAccountRequestDto accountRequestDto, CancellationToken token)
    {
        var userId = await this.GetUserId();
        if (userId == Guid.Empty)
            return BadRequest();

        var result = await this.CreateAccountAsync(accountRequestDto, userId, token);
        if (!result.IsSuccessful) return this.Error(result);
        
        //to do - make it CreatedAtAction
        return Ok(result);
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
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
        }

        return operationResult;
    }
}