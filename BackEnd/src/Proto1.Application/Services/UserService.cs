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
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Proto1.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;
    private readonly SignInManager<User> signInManager;
    private readonly IUserPersist userPersist;
    private readonly IRoleService roleService;

    public UserService(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IUserPersist userPersist, IRoleService roleService )
    {
        this.userManager = userManager;
        this.mapper = mapper;
        this.signInManager = signInManager;
        this.userPersist = userPersist;
        this.roleService = roleService;
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

    public async Task<List<UserCreateDto>> GetUsersAsync(){
        try
        {
            var usersList = await userPersist.GetUsersAsync();
            if (usersList == null) return null;

            return mapper.Map<List<UserCreateDto>>(usersList);
        }
        catch (Exception ex)
        {            
            throw new Exception("Error while trying to get users list: " + ex.Message);
        }
    }

    public async Task<bool> AssignRole(string userName, string roleName){
        try
        {
            var user = await userPersist.GetUserByUserNameAsync(userName);
            if (user == null) throw new Exception("invalid username");

            if (! await roleService.RoleExists(roleName)) throw new Exception("Invalid Role");

            if (!await userManager.IsInRoleAsync(user, roleName)){
                var result = await userManager.AddToRoleAsync(user, roleName);

                if (result.Succeeded){
                    return true;
                } else {
                    throw new Exception("Assignment to role not possible. Errors: " + result.Errors.ToString());
                }
            }
            return false;
            
        }
        catch (Exception ex)
        {            
            throw new Exception("Error while trying to assign role to user: " + ex.Message);
        }
    }

    public async Task AssignListOfRolesAsync(string userName, List<RoleUpdate> roles){
        try
        {
            var user = await userPersist.GetUserByUserNameAsync(userName) ?? throw new Exception("Invalid Username");

            if (!await CheckListOfRoles(roles)) throw new Exception("There was a problem in the list of roles provided");            

            foreach (RoleUpdate role in roles)
            {
                if (role.selected){
                    if (!await userManager.IsInRoleAsync(user, role.roleName)){
                        await userManager.AddToRoleAsync(user, role.roleName);
                    }
                } else {
                    if (await userManager.IsInRoleAsync(user, role.roleName)){
                        await userManager.RemoveFromRoleAsync(user, role.roleName);
                    }
                }
            }
        }
        catch (Exception ex)
        {            
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> RemoveRoleFromUser(string userName, string roleName){
        try
        {
            var user = await userPersist.GetUserByUserNameAsync(userName);
            if (user == null) throw new Exception("Invalid user");

            if (await userManager.IsInRoleAsync(user, roleName)){
                IdentityResult result = await userManager.RemoveFromRoleAsync(user, roleName);

                if (result.Succeeded) {
                    return true;
                } else {
                    throw new Exception("Error while trying to remove role from user. Error: " + result.Errors.ToString());
                }
            }
            return false;
        }
        catch (Exception ex) 
        {            
            throw new Exception(ex.Message);
        }
    }

    public async Task<IList<string>> GetUserRoles(string userName) {
        try
        {
            var user = await userPersist.GetUserByUserNameAsync(userName);
            if (user == null) return null;

            var roles = await userManager.GetRolesAsync(user);
            return roles;
        }
        catch (Exception ex)
        {            
            throw new Exception(ex.Message);
        }
    }

    public async Task<UserData> GetUserByIdAsync(int id){
        try
        {
            var user = await userPersist.GetUserByIdAsync(id);
            if (user == null) return null;

            UserData userData = mapper.Map<UserData>(user);
            userData.Roles = await userManager.GetRolesAsync(user);
            return userData;

        }
        catch (Exception ex)
        {            
            throw new Exception("Error while trying to get user by id. Error message:" + ex.Message);
        }
    }
    

    public async Task<bool> CheckListOfRoles(List<RoleUpdate> roles){
        
        foreach (RoleUpdate role in roles)
        {
            if (!await roleService.RoleExists(role.roleName)) 
                return false;                
        }
        return true;
    }
    
}
