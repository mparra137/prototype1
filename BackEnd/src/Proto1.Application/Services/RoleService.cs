using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proto1.Application.Contract;
using Proto1.Domain.Identity;

namespace Proto1.Application.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<Role> roleManager;

    public RoleService(RoleManager<Role> roleManager)
    {
        this.roleManager = roleManager;
    }

    public async Task<bool> CreateRoleAsync(string roleName){

        try
        {
            var role = new Role{
                Name = roleName
            };

            var result = await roleManager.CreateAsync(role);            
            return result.Succeeded;
        }
        catch (Exception ex)
        {            
            throw new Exception(ex.Message);
        }
        
    }

    public async Task<List<Role>> ListRoles(){
        try
        {
            var rolesList = await roleManager.Roles.ToListAsync();        
            if (rolesList == null) return  null;

            return rolesList;
        }
        catch (Exception ex)
        {            
            throw new Exception("Error while trying to retrieve roles list: " + ex.Message);
        }     
       
    }

    public async Task<bool> RoleExists2(string roleName){
        try{
            return await roleManager.RoleExistsAsync(roleName);
        }   
        catch (Exception ex){
            throw new Exception("Error while trying to check role existence: " + ex.Message);
        }
    }

    public async Task<bool> RoleExists(string roleName) => await roleManager.RoleExistsAsync(roleName);


    //public async Task<bool> CreateClaimAsync(){}
}
