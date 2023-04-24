using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proto1.Domain;
using Proto1.Persistence.Contract;
using Proto1.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Proto1.Persistence;

public class PessoaPersist : GeneralPersist, IPessoaPersist
{
    private readonly ProtoContext context;

    public PessoaPersist(ProtoContext context) : base(context)
    {
        this.context = context;        
    }
    public async Task<List<Pessoa>> GetPessoasAsync()
    {
        return await context.Pessoas.AsNoTracking().ToListAsync();
    }

    public async Task<Pessoa> GetByIdAsync(int pessoaId){
        return await context.Pessoas.AsNoTracking().SingleOrDefaultAsync(pessoa => pessoa.Id == pessoaId);
    }

    public async Task<Pessoa> GetByCPFAsync(string cpf){
        return await context.Pessoas.AsNoTracking().FirstOrDefaultAsync(pessoa => pessoa.CPF == cpf);
    }
}
