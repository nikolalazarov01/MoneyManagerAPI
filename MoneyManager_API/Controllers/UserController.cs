using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using Core.Contracts;
using Data.Models;
using Data.Models.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MoneyManager_API.Configuration;
using MoneyManager_API.Extensions;
using Utilities;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace MoneyManager_API.Controllers;

[ApiController]
[Route("/users")]
public class UserController : ControllerBase
{
    private readonly IUnitOfWork _services;
    private readonly IMapper _mapper;
    private readonly IValidator<RegisterRequestDto> _validator;

    public UserController(IUnitOfWork services, IMapper mapper, IValidator<RegisterRequestDto> validator)
    {
        _services = services;
        _mapper = mapper;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest, CancellationToken token)
    {
        var validationResult = await this.ValidateAsync(registerRequest, token);
        if (validationResult is { IsValid: false }) return this.ValidationError(validationResult);

        return await this.RegisterAsync(registerRequest, token);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(Guid id, CancellationToken token)
    {
        return Ok();
    }

    private async Task<ValidationResult> ValidateAsync(RegisterRequestDto registerRequest, CancellationToken token)
    {
        if (this._validator is null) return null;
        return await this._validator.ValidateAsync(registerRequest, token);
    }

    private async Task<IActionResult> RegisterAsync(RegisterRequestDto registerRequestDto, CancellationToken token)
    {
        var operationResult = new OperationResult<UserDto>();
        
        var result = await this._services.Users.RegisterAsync(registerRequestDto);
        if (!result.IsSuccessful) return this.Error(result);;

        var representation = this._mapper.Map<UserDto>(result.Data);
        
        return CreatedAtAction("GetById", new {Id = result.Data.Id}, representation);
    }
}