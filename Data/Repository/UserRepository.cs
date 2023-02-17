using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Data.Models;
using Data.Models.DTO;
using Data.Repository.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Data.Repository;

public class UserRepository : IUserRepository
{
    private readonly DbContext _db;
    private readonly UserManager<User> _userManager;
    private string secretKey;

    public UserRepository(DbContext db, UserManager<User> userManager, IConfiguration configuration)
    {
        _db = db;
        _userManager = userManager;
        secretKey = configuration.GetSection("ApiSettings")["SecretKey"];
    }

    public async Task<bool> IsUnique(string username)
    {
        if (!await _db.Set<User>().AnyAsync(u => u.UserName == username))
        {
            return false;
        }

        return true;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var user = await _db.Set<User>().FirstOrDefaultAsync(u => 
            String.Equals(u.UserName, loginRequestDto.Username, StringComparison.CurrentCultureIgnoreCase));

        bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

        if (user == null || !isValid)
        {
            return new LoginResponseDto
            {
                User = null,
                Token = ""
            };
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
        LoginResponseDto loginResponseDto = new LoginResponseDto()
        {
            Token = tokenHandler.WriteToken(token),
            User = user
        };

        return loginResponseDto;
    }
}