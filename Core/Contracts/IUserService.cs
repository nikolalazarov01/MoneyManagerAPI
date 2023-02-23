﻿using Data.Models;
using Utilities;

namespace Core.Contracts;

public interface IUserService
{
    Task<OperationResult<User>> GetUserById(Guid id, CancellationToken token);
    Task<OperationResult<Currency>> SetUserBaseCurrency(Currency currency, Guid userId, CancellationToken token);
}