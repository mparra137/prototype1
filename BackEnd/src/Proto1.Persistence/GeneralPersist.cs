using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proto1.Persistence.Contract;
using Proto1.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Proto1.Persistence;

public class GeneralPersist : IGeneralPersist
{
    private readonly ProtoContext context;

    public GeneralPersist(ProtoContext context)
    {
        this.context = context;
    }

    public async void Insert<T>(T entity) where T : class
    {
        await context.AddAsync<T>(entity);        
    }

    public void Update<T>(T entity) where T : class
    {
        context.Update<T>(entity);
    }

    public void Delete<T>(T entity) where T : class
    {
        context.Remove<T>(entity);
    }

    public void DeleteRange<T>(List<T> entities) where T : class
    {
        context.RemoveRange(entities);
    }    

    public async Task<bool> SaveChangesAsync()
    {
        return (await context.SaveChangesAsync()) > 0;
    }

    
}
