namespace Data.DtoModels;

public class RegisterRequestDtoModel
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public BaseCurrencyDtoModel Currency { get; set; }
}