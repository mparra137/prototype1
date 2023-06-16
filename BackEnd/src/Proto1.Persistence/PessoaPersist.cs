using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proto1.Domain;
using Proto1.Persistence.Contract;
using Proto1.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Proto1.Persistence.Models;

namespace Proto1.Persistence;

public class PessoaPersist : GeneralPersist, IPessoaPersist
{
    private readonly ProtoContext context;

    public PessoaPersist(ProtoContext context) : base(context)
    {
        this.context = context;        
    }
    public async Task<PageList<Pessoa>> GetAllAsync(PageParams pageParams)
    {
        IQueryable<Pessoa> query = context.Pessoas; 

        if (!String.IsNullOrEmpty(pageParams.Term)){
            query = query.Where(p => p.Nome.ToLower().Contains(pageParams.Term.ToLower())
                                  || p.CPF.Contains(pageParams.Term)
                                  || p.Endereco.ToLower().Contains(pageParams.Term.ToLower())
                                  || p.Cidade.ToLower().Contains(pageParams.Term.ToLower())
                                  || p.Bairro.ToLower().Contains(pageParams.Term.ToLower())
                                  || p.UF.ToLower().Contains(pageParams.Term)  
            );
        }

        query = query.AsNoTracking();

        return await PageList<Pessoa>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);         
        
        //return await query.AsNoTracking().ToListAsync();

    }

    public async Task<Pessoa> GetByIdAsync(int pessoaId){
        return await context.Pessoas.AsNoTracking().SingleOrDefaultAsync(pessoa => pessoa.Id == pessoaId);
    }

    public async Task<Pessoa> GetByCPFAsync(string cpf){
        return await context.Pessoas.AsNoTracking().FirstOrDefaultAsync(pessoa => pessoa.CPF == cpf);
    }
}
