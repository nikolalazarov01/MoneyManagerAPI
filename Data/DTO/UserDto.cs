using Data.Models.DTO.Base;

namespace Data.Models.DTO;

public class UserDto : BaseDtoModel
{
    public CurrencyDto Currency { get; set; }
}