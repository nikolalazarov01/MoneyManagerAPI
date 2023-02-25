using AutoMapper;
using Core.Contracts;
using Microsoft.AspNetCore.Mvc;

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
}