using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proto1.Application.Contract
{
    public interface IRoleService
    {
        Task<bool> CreateRoleAsync(string roleName);
    }
}