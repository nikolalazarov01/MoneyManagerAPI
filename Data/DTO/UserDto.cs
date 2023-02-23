using Data.Models.DTO.Base;

namespace Data.Models.DTO;

public class UserDto : BaseDtoModel
{
    public BaseCurrencyDto BaseCurrency { get; set; }
}