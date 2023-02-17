using AutoMapper;
using Data.Models;
using Data.Models.DTO;

namespace MoneyManager_API.MappingConfigs;

public class UserMappingConfig : Profile
{
    public UserMappingConfig()
    {
        this.CreateMap<UserDto, User>().ReverseMap();
    }
}