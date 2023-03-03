using AutoMapper;
using Core.Contracts;
using Data.Models.DTO.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager_API.Extensions;

namespace MoneyManager_API.Controllers;

[ApiController]
public class TransactionController : ControllerBase
{
    private readonly IUnitOfWork _services;
    private readonly IMapper _mapper;

    public TransactionController(IUnitOfWork services, IMapper mapper)
    {
        _services = services;
        _mapper = mapper;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> MakeTransaction([FromBody] AccountInfoRequestDto accountInfoDto, CancellationToken token)
    {
        var validationResult = await this._services.Accounts.ValidateTransaction(accountInfoDto, token);
        if (!validationResult.IsSuccessful) return this.Error(validationResult);

        var result = await this._services.Accounts.MakeTransaction(accountInfoDto, token);
        if (!result.IsSuccessful) return this.Error(result);

        var representation = this._mapper.Map<AccountDto>(result.Data);

        return Ok(representation);
    }
}