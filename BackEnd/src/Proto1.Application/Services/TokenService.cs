using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proto1.Domain.Identity;
using Proto1.Application.Contract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Proto1.Application.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Security.Claims;

namespace Proto1.Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration config;
    private readonly IMapper mapper;
    private readonly UserManager<User> userManager;
    private readonly SymmetricSecurityKey key;

    public TokenService(IConfiguration config, IMapper mapper, UserManager<User> userManager)
    {
        this.config = config;
        this.mapper = mapper;
        this.userManager = userManager;
        this.key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
    }


    public async Task<string> GenerateToken(UserCreateDto userDto)
    {
        var user = mapper.Map<User>(userDto);

        var claims = new List<Claim>{
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var roles = await userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescription = new SecurityTokenDescriptor{
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescription);

        return tokenHandler.WriteToken(token);

    }
}
