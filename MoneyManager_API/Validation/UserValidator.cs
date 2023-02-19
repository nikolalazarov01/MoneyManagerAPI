using Data.Models.DTO;
using FluentValidation;

namespace MoneyManager_API.Validation;

public class UserValidator : AbstractValidator<RegisterRequestDto>
{
    public UserValidator()
    {
        this.RuleFor(u => u.UserName).NotEmpty();
        this.RuleFor(u => u.Password).NotEmpty();
        this.RuleFor(u => u.Email).NotEmpty();
        this.RuleFor(u => u.Currency).NotEmpty();
    }
}