using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Core.Contracts;
using Data.Models.DTO.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager_API.Extensions;

namespace MoneyManager_API.Controllers;

[ApiController]
[Route("/transaction")]
public class TransactionController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;

    public TransactionController(IAccountService accountService, IMapper mapper)
    {
        _accountService = accountService;
        _mapper = mapper;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> MakeTransaction([FromBody] AccountInfoRequestDto accountInfoDto, CancellationToken token)
    {
        var validationResult = await this._accountService.ValidateTransaction(accountInfoDto, token);
        if (!validationResult.IsSuccessful) return this.Error(validationResult);

        var result = await this._accountService.MakeTransaction(accountInfoDto, token);
        if (!result.IsSuccessful) return this.Error(result);

        var representation = this._mapper.Map<AccountDto>(result.Data);

        return Ok(representation);
    }
}