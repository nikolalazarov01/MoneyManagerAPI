﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
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
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private string secretKey;

    public UserRepository(DbContext db, UserManager<User> userManager, IConfiguration configuration, IMapper mapper, RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _userManager = userManager;
        _mapper = mapper;
        _roleManager = roleManager;
        secretKey = configuration.GetSection("ApiSettings")["SecretKey"];
    }

    public async Task<OperationResult> IsUnique(string username)
    {
        var operationResult = new OperationResult();
        if (!await _db.Set<User>().AnyAsync(u => u.UserName == username))
        {
            operationResult.AddError(new Error{IsNotExpected = false, Message = "User with that username already exists!"});
            return operationResult;
        }
        
        return operationResult;
    }

    public async Task<OperationResult<LoginResponseDto>> Login(string username, string password)
    {
        var operationResult = new OperationResult<LoginResponseDto>();
        
        var user = await _db.Set<User>().FirstOrDefaultAsync(u => 
            u.UserName.ToLower() == username.ToLower());

        bool isValid = await _userManager.CheckPasswordAsync(user, password);

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

    public async Task<OperationResult<User>> Register(User user, string password)
    {
        var operationResult = new OperationResult<User>();
        
        try
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync(UserRoles.Admin.ToString()).GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin.ToString()));
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.BasicUser.ToString()));
                }
                await _userManager.AddToRoleAsync(user, UserRoles.BasicUser.ToString());
                var userToReturn =
                    await _db.Set<User>().FirstOrDefaultAsync(u => u.UserName == user.UserName);
                operationResult.Data = userToReturn;
                return operationResult;
            }
        }
        catch (Exception ex)
        {
            operationResult.AppendException(ex);
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