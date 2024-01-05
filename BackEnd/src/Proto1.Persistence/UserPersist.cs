using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proto1.Persistence.Contract;
using Proto1.Persistence.Context;
using Proto1.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace Proto1.Persistence;

public class UserPersist : IUserPersist
{
    private readonly ProtoContext context;
    public UserPersist(ProtoContext context)
    {
        this.context = context;            
    }

    public async Task<User> GetUserByUserNameAsync(string userName)
    {
        return await context.Users.SingleOrDefaultAsync(user => user.UserName == userName);
    }

    public async Task<List<User>> GetUsersAsync(){
        var query = context.Users;
        return await query.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int id){
        var query = context.Users;
        return await query.FirstOrDefaultAsync(u => u.Id == id);
    }
}
