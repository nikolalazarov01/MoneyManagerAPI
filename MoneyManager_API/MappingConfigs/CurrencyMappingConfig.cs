using AutoMapper;
using Data.Models;
using Data.Models.DTO;

namespace MoneyManager_API.MappingConfigs;

public class CurrencyMappingConfig : Profile
{
    public CurrencyMappingConfig()
    {
        this.CreateMap<BaseCurrencyDto, Currency>().ReverseMap();
    }
}