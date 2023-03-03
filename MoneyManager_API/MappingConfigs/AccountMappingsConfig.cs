using AutoMapper;
using Data.Models;
using Data.Models.DTO.Account;

namespace MoneyManager_API.MappingConfigs;

public class AccountMappingsConfig : Profile
{
    public AccountMappingsConfig()
    {
        this.CreateMap<NewAccountCreatedDto, Account>().ReverseMap();
        this.CreateMap<NewAccountRequestDto, Account>().ReverseMap();
        this.CreateMap<UserAccountDto, Account>().ReverseMap();
        this.CreateMap<AccountDto, Account>().ReverseMap();

        this.CreateMap<AccountInfoDto, AccountInfo>().ReverseMap();
    }
}