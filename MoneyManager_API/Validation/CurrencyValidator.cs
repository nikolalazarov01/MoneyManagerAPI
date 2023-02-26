﻿using Data.Models.DTO;
using FluentValidation;

namespace MoneyManager_API.Validation;

public class CurrencyValidator : AbstractValidator<CurrencyDto>
{
    public CurrencyValidator()
    {
        this.RuleFor(u => u.Code).NotEmpty();
    }
}