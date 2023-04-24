using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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

    //public async Task<bool> CreateClaimAsync(){}
}
