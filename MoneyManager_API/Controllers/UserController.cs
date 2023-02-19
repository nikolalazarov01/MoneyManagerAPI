using AutoMapper;
using Core.Contracts;
using Data.Models.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace MoneyManager_API.Controllers;

[ApiController]
[Route("/users")]
public class UserController : ControllerBase
{
    private readonly IUnitOfWork _services;
    private readonly IMapper _mapper;
    private readonly IValidator<UserDto> _validator;

    public UserController(IUnitOfWork services, IMapper mapper, IValidator<UserDto> validator)
    {
        _services = services;
        _mapper = mapper;
        _validator = validator;
    }
    
    
}