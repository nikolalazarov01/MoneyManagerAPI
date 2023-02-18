﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Data.DtoModels;
using Data.Models;
using Data.Models.DTO;
using Data.Repository.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Utilities;

namespace Data.Repository;

public class UserRepository : IUserRepository
{
    private readonly DbContext _db;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private string secretKey;

    public UserRepository(DbContext db, UserManager<User> userManager, IConfiguration configuration, IMapper mapper)
    {
        _db = db;
        _userManager = userManager;
        _mapper = mapper;
        secretKey = configuration.GetSection("ApiSettings")["SecretKey"];
    }

    public async Task<OperationResult<bool>> IsUnique(string username)
    {
        var operationResult = new OperationResult<bool>();
        if (!await _db.Set<User>().AnyAsync(u => u.UserName == username))
        {
            operationResult.Data = false;
            return operationResult;
        }

        operationResult.Data = true;
        return operationResult;
    }

    public async Task<OperationResult<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
    {
        var operationResult = new OperationResult<LoginResponseDto>();
        
        var user = await _db.Set<User>().FirstOrDefaultAsync(u => 
            String.Equals(u.UserName, loginRequestDto.Username, StringComparison.CurrentCultureIgnoreCase));

        bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

        if (user == null || !isValid)
        {
            Error error = new Error
            {
                IsNotExpected = false,
                Message = "User not found"
            };
            operationResult.AddError(error);
            return operationResult;
        }

        var roles = await _userManager.GetRolesAsync(user);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(5),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        foreach (var role in roles)
        {
            tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        var token = tokenHandler.CreateToken(tokenDescriptor);
        LoginResponseDto loginResponseDto = new LoginResponseDto
        {
            Token = tokenHandler.WriteToken(token),
            UserName = user.UserName
        };

        operationResult.Data = loginResponseDto;
        return operationResult;
    }

    public async Task<OperationResult<UserDto>> Register(RegisterRequestDto registerRequestDto)
    {
        var operationResult = new OperationResult<UserDto>();
        
        User user = new User
        {
            UserName = registerRequestDto.UserName,
            Email = registerRequestDto.Email,
            NormalizedEmail = registerRequestDto.Email.ToUpper(),
            BaseCurrency = new Currency(registerRequestDto.Currency.Code)
        };
        try
        {
            var result = await _userManager.CreateAsync(user, registerRequestDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.BasicUser.ToString());
                var userToReturn =
                    await _db.Set<User>().FirstOrDefaultAsync(u => u.UserName == registerRequestDto.UserName);
                operationResult.Data = _mapper.Map<UserDto>(userToReturn);
                return operationResult;
            }
        }
        catch (Exception ex)
        {
            Error error = new Error
            {
                IsNotExpected = true,
                Message = ex.Message
            };
            operationResult.AddError(error);
            return operationResult;
        }
        
        
        operationResult.AddError(new Error
        {
            IsNotExpected = true,
            Message = "Error in register"
        });
        return operationResult;
    }
}