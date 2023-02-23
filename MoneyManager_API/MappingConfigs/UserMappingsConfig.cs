using AutoMapper;
using Data.Models;
using Data.Models.DTO;

namespace MoneyManager_API.MappingConfigs;

public class UserMappingsConfig : Profile
{
    public UserMappingsConfig()
    {
        this.CreateMap<UserDto, User>().ReverseMap();
    }
}