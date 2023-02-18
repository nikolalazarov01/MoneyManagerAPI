﻿using Data.Models;
using Data.Models.DTO;

namespace Data.Repository.Contracts;

public interface IUserRepository
{
    Task<bool> IsUnique(string username);
    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    Task<UserDto> Register(RegisterRequestDto registerRequestDto);
}