using AutoMapper;
using Data.DtoModels;
using Data.Models.DTO;

namespace MoneyManager_API.MappingConfigs;

public class LoginMappingConfig : Profile
{
    public LoginMappingConfig()
    {
        this.CreateMap<LoginRequestDto, LoginRequestDtoModel>().ReverseMap();
        this.CreateMap<LoginRequestDto, LoginResponseDtoModel>().ReverseMap();
    }
}