﻿using Data.Models;
using Data.Models.DTO;
using Utilities;

namespace Data.Repository.Contracts;

public interface IUserRepository
{
    Task<OperationResult> IsUnique(string username);
    Task<OperationResult<LoginResponseDto>> Login(string username, string password);
    Task<OperationResult<UserDto>> Register(User user, string password);
}