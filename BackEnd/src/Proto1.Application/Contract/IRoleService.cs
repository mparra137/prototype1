using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proto1.Domain.Identity;

namespace Proto1.Application.Contract
{
    public interface IRoleService
    {
        Task<bool> CreateRoleAsync(string roleName);
        Task<List<Role>> ListRoles();
        Task<bool> RoleExists(string roleName);
    }
}