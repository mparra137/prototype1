using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proto1.Application.Contract;
using Proto1.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Proto1.Application.DTOs;
using AutoMapper;
using Proto1.Persistence.Contract;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Proto1.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;
    private readonly SignInManager<User> signInManager;
    private readonly IUserPersist userPersist;

    public UserService(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IUserPersist userPersist )
    {
        this.userManager = userManager;
        this.mapper = mapper;
        this.signInManager = signInManager;
        this.userPersist = userPersist;
    }
    public async Task<SignInResult> CheckUserPasswordAsync( string userName, string password)
    {
        try
        {
            //var user = await userManager.Users.SingleOrDefaultAsync(user => user.UserName == userName);
            var user = await userPersist.GetUserByUserNameAsync(userName);
            return await signInManager.CheckPasswordSignInAsync(user, password, false);            
        }
        catch (Exception ex)
        {            
            throw new Exception("Error trying to check user's password: " + ex.Message);
        }
    }

    public async Task<UserCreateDto> CreateAccountAsync(UserCreateDto userDto)
    {
        try
        {
            var user = mapper.Map<User>(userDto);
            var result = await userManager.CreateAsync(user, userDto.Password);                        

            if (result.Succeeded){    
                Claim claim = new Claim("Active", "true");
                await userManager.AddClaimAsync(user, claim);                  

                var userCreated = mapper.Map<UserCreateDto>(user);
                return userCreated;
            }
            return null;
        }
        catch (Exception ex)
        {            
            throw new Exception(ex.Message);
        }
    }

    public async Task<UserCreateDto> GetUserByUsernameAsync(string userName)
    {
        try
        {
            var user = await userPersist.GetUserByUserNameAsync(userName);
            if (user == null) return null;

            return mapper.Map<UserCreateDto>(user);
        }
        catch (Exception ex)
        {            
            throw new Exception(ex.Message);
        }
    }
    
}
