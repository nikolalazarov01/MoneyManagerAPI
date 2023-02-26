using Data.Models.DTO.Account;
using Data.Models.DTO.Base;

namespace Data.Models.DTO;

public class UserDto : BaseDtoModel
{
    public Guid Id { get; set; }
    public CurrencyDto Currency { get; set; }
    public ICollection<UserAccountDto> Accounts { get; set; }
}