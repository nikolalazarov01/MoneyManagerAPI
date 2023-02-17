using Data.Models;

namespace Data.DtoModels;

public class LoginResponseDtoModel
{
    public string? UserName { get; set; }
    public string Token { get; set; }
}