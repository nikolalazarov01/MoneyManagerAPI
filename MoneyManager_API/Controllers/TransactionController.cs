using AutoMapper;
using Core.Contracts;
using Microsoft.AspNetCore.Mvc;

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
}