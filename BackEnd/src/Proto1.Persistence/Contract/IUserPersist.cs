using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proto1.Domain.Identity;

namespace Proto1.Persistence.Contract
{
    public interface IUserPersist
    {
        Task<User> GetUserByUserNameAsync(string userName); 
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
    };
}